using System.Text.Json.Serialization;

namespace NewWorldLib.Datasheets;

[JsonConverter(typeof(DatasheetPropertyJsonConverter))]
public abstract class DatasheetProperty
{
    public string Name { get; init; }

    public int? ToIntValue()
    {
        if (this is DatasheetIntProperty intProperty)
        {
            return intProperty.Value;
        }

        return null;
    }

    public float? ToFloatValue()
    {
        if (this is DatasheetFloatProperty floatProperty)
        {
            return floatProperty.Value;
        }

        return null;
    }

    public string ToStringValue()
    {
        if (this is DatasheetStringProperty stringProperty)
        {
            return stringProperty.Value;
        }

        return null;
    }
}