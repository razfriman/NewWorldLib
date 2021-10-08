using System.Text.Json.Serialization;

namespace NewWorldLib.Datasheets;

public class DatasheetMetadata
{
    [JsonIgnore]
    public int Crc32 { get; set; }
    [JsonIgnore]
    public int Unknown1 { get; set; }
    public int ColumnCount { get; set; }
    public int RowCount { get; set; }
    [JsonIgnore]
    public int Unknown2 { get; set; }
    [JsonIgnore]
    public int Unknown3 { get; set; }
    [JsonIgnore]
    public int Unknown4 { get; set; }
    [JsonIgnore]
    public int Unknown5 { get; set; }
}