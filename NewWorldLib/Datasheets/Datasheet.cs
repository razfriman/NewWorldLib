using NewWorldLib.Datasheets.Internal;
using NewWorldLib.Extensions;

namespace NewWorldLib.Datasheets;

public class Datasheet
{
    public List<Dictionary<string, DatasheetProperty>> Items { get; } = new();

    public static Datasheet Parse(string path)
    {
        using var stream = File.OpenRead(path);
        return Parse(stream);
    }

    public static Datasheet Parse(Stream stream)
    {
        using var reader = new BinaryReader(stream);
        var datasheet = new Datasheet();

        var header = ParseHeader(reader);
        var metadata = ParseMetadata(reader);
        var columns = ParseColumns(reader, metadata, header);
        var rows = ParseRows(reader, metadata, header);

        foreach (var row in rows)
        {
            var item = new Dictionary<string, DatasheetProperty>();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                var columnValue = row.Cells[i];
                item[column.ColumnName] = column.ColumnType switch
                {
                    DatasheetColumnType.Int => new DatasheetIntProperty
                    {
                        Name = column.ColumnName,
                        Value = columnValue.ValueString == "" ? null : int.Parse(columnValue.ValueString)
                    },
                    DatasheetColumnType.Float => new DatasheetFloatProperty
                    {
                        Value = columnValue.ValueString == "" ? null : float.Parse(columnValue.ValueString)
                    },
                    DatasheetColumnType.String => new DatasheetStringProperty
                    {
                        Value = columnValue.ValueString == "" ? null : columnValue.ValueString
                    },
                    _ => item[column.ColumnName]
                };
            }

            datasheet.Items.Add(item);
        }

        return datasheet;
    }


    public static DatasheetHeader ParseHeader(BinaryReader reader)
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


    public static DatasheetMetadata ParseMetadata(BinaryReader reader) => new()
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

    public static List<DatasheetColumn> ParseColumns(BinaryReader reader, DatasheetMetadata metadata,
        DatasheetHeader header)
    {
        var result = new List<DatasheetColumn>();

        for (var i = 0; i < metadata.ColumnCount; i++)
        {
            var column = new DatasheetColumn
            {
                Unknown1 = reader.ReadInt32(),
                ColumnNameOffset = reader.ReadInt32(),
                ColumnType = (DatasheetColumnType)reader.ReadInt32(),
            };
            column.ColumnName = reader.ReadNullTerminatedString(column.ColumnNameOffset, header);
            result.Add(column);
        }

        return result;
    }

    public static List<DatasheetRow> ParseRows(BinaryReader reader, DatasheetMetadata metadata, DatasheetHeader header)
    {
        var result = new List<DatasheetRow>();

        for (var i = 0; i < metadata.RowCount; i++)
        {
            var row = new DatasheetRow();
            for (var j = 0; j < metadata.ColumnCount; j++)
            {
                var cell = new DatasheetCell
                {
                    ValueOffset = reader.ReadInt32(),
                    Unknown1 = reader.ReadInt32(),
                };
                cell.ValueString = reader.ReadNullTerminatedString(cell.ValueOffset, header);
                row.Cells.Add(cell);
            }
            result.Add(row);
        }

        return result;
    }
}