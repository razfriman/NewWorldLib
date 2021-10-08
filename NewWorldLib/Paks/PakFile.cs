using System.Data;
using NewWorldLib.Extensions;

namespace NewWorldLib;

public class PakFile : IDisposable
{
    public long CentralOffset { get; set; }
    public ushort CentralEntries { get; set; }
    public Dictionary<string, PakFileEntry> Entries { get; } = new();
    public BinaryReader Reader { get; set; }

    public static PakFile Parse(string path)
    {
        using var stream = File.OpenRead(path);
        return Parse(stream);
    }

    public static PakFile Parse(Stream stream)
    {
        var pakFile = new PakFile
        {
            Reader = new BinaryReader(stream)
        };
        pakFile.ParseHeader();
        pakFile.ParseEntries();
        return pakFile;
    }

    private void ParseHeader()
    {
        Reader.BaseStream.Seek(-22, SeekOrigin.End);
        var headerId = PakFileEntry.ParseHeaderId(Reader);
        var diskNum = Reader.ReadInt16();
        var diskStart = Reader.ReadInt16();
        CentralEntries = Reader.ReadUInt16();
        var centralEntries2 = Reader.ReadInt16();
        var centralSize = Reader.ReadInt32();
        CentralOffset = Reader.ReadInt32();
        var commentLength = Reader.ReadInt16();
        var comment = Reader.ReadString(commentLength);
    }

    private void ParseEntries()
    {
        Reader.BaseStream.Seek(CentralOffset, SeekOrigin.Begin);
        for (var i = 0; i < CentralEntries; i++)
        {
            var entry = PakFileEntry.Parse(Reader);
            if (entry == null)
            {
                break;
            }

            Entries.Add(entry.Name, entry);
        }
    }

    public void Save(string path)
    {
        foreach (var entry in Entries.Values)
        {
            try
            {
                entry.Save(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public void Dispose()
    {
        Reader?.Dispose();
        foreach (var entry in Entries.Values)
        {
            entry?.Dispose();
        }
    }
}