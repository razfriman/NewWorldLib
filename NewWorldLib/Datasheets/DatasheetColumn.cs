using System.Text.Json.Serialization;

namespace NewWorldLib.Datasheets;

public class DatasheetColumn
{
    [JsonIgnore]
    public int Unknown1 { get; set; }
    public int ColumnNameOffset { get; set; }
    public int ColumnType { get; set; }
    public string ColumnName { get; set; }
}