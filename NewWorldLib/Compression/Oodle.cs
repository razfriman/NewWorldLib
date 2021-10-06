namespace NewWorldLib.Compression;

public static class Oodle
{
    private static readonly Kraken Kraken = new Kraken();

    public static byte[] DecompressReplayData(byte[] buffer, int uncompressedSize) =>
        Kraken.Decompress(buffer, uncompressedSize);
}