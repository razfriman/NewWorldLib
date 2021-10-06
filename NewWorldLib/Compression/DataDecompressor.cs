using System.IO.Compression;

namespace NewWorldLib.Compression;

public class DataDecompressor
{
    public static byte[] DecompressOodle(byte[] compressedData, int decompressedLength)
    {
        var decompressed = new byte[decompressedLength];
        Oodle.Decompress(compressedData, 0, compressedData.Length, decompressed, 0, decompressedLength);
        return decompressed;
    }

    public static byte[] DecompressDeflate(byte[] compressedData, int decompressedLength)
    {
        using var decompressedStream = new MemoryStream();
        using (var compressStream = new MemoryStream(compressedData))
        {
            using (var deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress))
            {
                deflateStream.CopyTo(decompressedStream);
            }
        }

        return decompressedStream.ToArray();
    }
}