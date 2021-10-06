using System.Data;
using NewWorldLib.Compression;
using NewWorldLib.Extensions;

namespace NewWorldLib;

public class PakFileEntry : IDisposable
{
    public string Name { get; set; }
    public int Method { get; set; }
    public long Position { get; set; }
    public int Length { get; set; }
    public int DecompressedLength { get; set; }
    public bool IsLoaded { get; set; }
    public byte[] Data { get; set; }
    public BinaryReader Reader { get; set; }

    public static PakFileEntry Parse(BinaryReader reader)
    {
        var fileEntry = new PakFileEntry
        {
            Reader = reader
        };
        return fileEntry.Parse() ? fileEntry : null;
    }

    public void Load()
    {
        if (Data != null)
        {
            return;
        }

        if (Reader == null)
        {
            return;
        }

        Reader.BaseStream.Seek(Position, SeekOrigin.Begin);
        if (!Parse())
        {
            return;
        }

        var data = Reader.ReadBytes(Length);
        Data = Method switch
        {
            0 => data,
            8 => DataDecompressor.DecompressDeflate(data, DecompressedLength),
            15 => DataDecompressor.DecompressOodle(data, DecompressedLength),
            _ => throw new DataException($"Unknown compression method: {Method}")
        };
        IsLoaded = true;
    }


    public static short ParseHeaderId(BinaryReader reader)
    {
        var header = reader.ReadString(2);
        if (header != "PK")
        {
            throw new DataException($"Invalid Header Id: {header}");
        }

        var headerId = reader.ReadInt16();
        return headerId;
    }

    public bool Parse()
    {
        var position = Reader.BaseStream.Position;
        var headerId = ParseHeaderId(Reader);

        switch (headerId)
        {
            case 0x02_01:
            {
                var verMade = Reader.ReadInt16();
                var verNeed = Reader.ReadInt16();
                var flag = Reader.ReadInt16();
                var method = Reader.ReadInt16();
                var modTime = Reader.ReadInt16();
                var modDate = Reader.ReadInt16();
                var crc = Reader.ReadInt32();
                Length = Reader.ReadInt32();
                DecompressedLength = Reader.ReadInt32();
                var nameLen = Reader.ReadInt16();
                var extraLen = Reader.ReadInt16();
                var commLen = Reader.ReadInt16();
                var disknum = Reader.ReadInt16();
                var intAttr = Reader.ReadInt16();
                var ext_attr = Reader.ReadInt32();
                Position = Reader.ReadInt32();
                Name = Reader.ReadString(nameLen);
                var extra = Reader.ReadString(extraLen);
                var comment = Reader.ReadString(commLen);
                return true;
            }
            case 0x04_03:
            {
                var ver = Reader.ReadInt16();
                var flag = Reader.ReadInt16();
                Method = Reader.ReadInt16();
                var modTime = Reader.ReadInt16();
                var modDate = Reader.ReadInt16();
                var crc = Reader.ReadInt32();
                var compSize = Reader.ReadInt32();
                var uncompSize = Reader.ReadInt32();
                var nameLen = Reader.ReadInt16();
                var extraLen = Reader.ReadInt16();
                var name = Reader.ReadString(nameLen);
                var extra = Reader.ReadString(extraLen);
                return true;
            }
            case 0x06_05:
                return false;
            default:
                throw new DataException($"Invalid Header Data: {headerId}");
        }
    }

    public void Save(string path)
    {
        if (!IsLoaded)
        {
            Load();
        }

        var fullPath = Path.Combine(path, Name);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllBytes(fullPath, Data);
    }

    public void Dispose() => Reader?.Dispose();
}