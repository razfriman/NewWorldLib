using NewWorldLib.Extensions;

namespace NewWorldLib.Datasheets;

public class Datasheet
{
    public DatasheetHeader Header { get; set; }
    public DatasheetMetadata Metadata { get; set; }
    public List<DatasheetColumn> Columns { get; set; } = new();
    public List<DatasheetRow> Rows { get; set; } = new();

    public static Datasheet Parse(string path)
    {
        using var stream = File.OpenRead(path);
        return Parse(stream);
    }

    public static Datasheet Parse(Stream stream)
    {
        var datasheet = new Datasheet();
        using var reader = new BinaryReader(stream);
        datasheet.Parse(reader);
        return datasheet;
    }

    public void Parse(BinaryReader reader)
    {
        Header = ParseHeader(reader);
        Metadata = ParseMetadata(reader);
        Columns = ParseColumns(reader);
        Rows = ParseRows(reader);
    }

    public DatasheetHeader ParseHeader(BinaryReader reader)
    {
        var header = new DatasheetHeader
        {
            Version = reader.ReadInt32(),
            Unknown1 = reader.ReadInt32(),
            UniqueIdOffset = reader.ReadInt32(),
            Unknown2 = reader.ReadInt32(),
            DataTypeOffset = reader.ReadInt32(),
            RowNumber = reader.ReadInt32(),
            PlainTextLength = reader.ReadInt32(),
            Unknown3 = reader.ReadInt32(),
            Unknown4 = reader.ReadInt32(),
            Unknown5 = reader.ReadInt32(),
            Unknown6 = reader.ReadInt32(),
            Unknown7 = reader.ReadInt32(),
            Unknown8 = reader.ReadInt32(),
            Unknown9 = reader.ReadInt32(),
            PlainTextOffset = reader.ReadInt32(),
        };
        header.UniqueId = reader.ReadNullTerminatedString(header.UniqueIdOffset, header);
        header.DataType = reader.ReadNullTerminatedString(header.DataTypeOffset, header);
        return header;
    }


    public DatasheetMetadata ParseMetadata(BinaryReader reader) => new()
    {
        Crc32 = reader.ReadInt32(),
        Unknown1 = reader.ReadInt32(),
        ColumnCount = reader.ReadInt32(),
        RowCount = reader.ReadInt32(),
        Unknown2 = reader.ReadInt32(),
        Unknown3 = reader.ReadInt32(),
        Unknown4 = reader.ReadInt32(),
        Unknown5 = reader.ReadInt32(),
    };

    public List<DatasheetColumn> ParseColumns(BinaryReader reader)
    {
        var result = new List<DatasheetColumn>();

        for (var i = 0; i < Metadata.ColumnCount; i++)
        {
            var column = new DatasheetColumn
            {
                Unknown1 = reader.ReadInt32(),
                ColumnNameOffset = reader.ReadInt32(),
                ColumnType = reader.ReadInt32(),
            };
            column.ColumnName = reader.ReadNullTerminatedString(column.ColumnNameOffset, Header);
            result.Add(column);
        }

        return result;
    }

    public List<DatasheetRow> ParseRows(BinaryReader reader)
    {
        var result = new List<DatasheetRow>();

        for (var i = 0; i < Metadata.RowCount; i++)
        {
            var row = new DatasheetRow();
            for (var j = 0; j < Metadata.ColumnCount; j++)
            {
                var cell = new DatasheetCell
                {
                    ValueOffset = reader.ReadInt32(),
                    ValueDuplicate = reader.ReadInt32(),
                };
                cell.Value = reader.ReadNullTerminatedString(cell.ValueOffset, Header);
                row.Cells.Add(cell);
            }

            result.Add(row);
        }

        return result;
    }
}