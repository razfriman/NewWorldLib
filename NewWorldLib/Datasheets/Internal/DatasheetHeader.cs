namespace NewWorldLib.Datasheets.Internal;

public class DatasheetHeader
{
    public int HeaderSize => 60;
    public int Version { get; set; }
    public int Unknown1 { get; set; }
    public int UniqueIdOffset { get; set; }
    public int Unknown2 { get; set; }
    public int DataTypeOffset { get; set; }
    public int RowNumber { get; set; }
    public int PlainTextLength { get; set; }
    public int Unknown3 { get; set; }
    public int Unknown4 { get; set; }
    public int Unknown5 { get; set; }
    public int Unknown6 { get; set; }
    public int Unknown7 { get; set; }
    public int Unknown8 { get; set; }
    public int Unknown9 { get; set; }
    public int PlainTextOffset { get; set; }
    public string UniqueId { get; set; }
    public string DataType { get; set; }
}