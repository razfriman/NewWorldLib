using System.Text;

namespace NewWorldLib.Extensions;

public static class BinaryReaderExtensions
{
    public static string ReadString(this BinaryReader reader, int bytes) =>
        Encoding.ASCII.GetString(reader.ReadBytes(bytes));
}