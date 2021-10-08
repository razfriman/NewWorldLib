using System.Text;
using NewWorldLib.Datasheets;

namespace NewWorldLib.Extensions;

public static class BinaryReaderExtensions
{
    public static string ReadString(this BinaryReader reader, int bytes) =>
        Encoding.ASCII.GetString(reader.ReadBytes(bytes));
    
    public static string ReadNullTerminatedString(this BinaryReader reader)
    {
        var str = "";
        char ch;
        while ((ch = reader.ReadChar()) != 0)
            str += ch;
        return str;
    }
    
    public static string ReadNullTerminatedString(this BinaryReader reader, int fieldOffset, int plainTextOffset, int headerSize) => reader.ReadNullTerminatedString(fieldOffset + plainTextOffset + headerSize);
    
    public static string ReadNullTerminatedString(this BinaryReader reader, int fieldOffset, DatasheetHeader header) => reader.ReadNullTerminatedString(fieldOffset + header.PlainTextOffset + header.HeaderSize);

    public static string ReadNullTerminatedString(this BinaryReader reader, int offset)
    {
        var position = reader.BaseStream.Position;
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        var result = reader.ReadNullTerminatedString();
        reader.BaseStream.Seek(position, SeekOrigin.Begin);
        return result;
    }
}