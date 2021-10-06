using System.Data;
using System.Runtime.InteropServices;

namespace NewWorldLib.Compression;

public static class Oodle
{
    public const string OODLE_DLL_NAME = "oo2core_8_win64.dll";

    public static unsafe void Decompress(byte[] compressed, int compressedOffset, int compressedSize,
        byte[] uncompressed, int uncompressedOffset, int uncompressedSize)
    {
        long decodedSize;

        fixed (byte* compressedPtr = compressed, uncompressedPtr = uncompressed)
        {
            decodedSize = OodleLZ_Decompress(compressedPtr + compressedOffset, compressedSize,
                uncompressedPtr + uncompressedOffset, uncompressedSize, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);
        }

        if (decodedSize <= 0)
        {
            throw new DataException($"Oodle decompression failed with result {decodedSize}");
        }
    }

    [DllImport(OODLE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern long OodleLZ_Decompress(byte[] buffer, long bufferSize, byte[] output, long outputBufferSize,
        int a, int b, int c, long d, long e, long f, long g, long h, long i, int threadModule);

    [DllImport(OODLE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe long OodleLZ_Decompress(byte* buffer, long bufferSize, byte* output,
        long outputBufferSize, int a, int b, int c, long d, long e, long f, long g, long h, long i, int threadModule);
}