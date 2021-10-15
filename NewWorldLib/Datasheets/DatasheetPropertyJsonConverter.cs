using System.Text.Json;
using System.Text.Json.Serialization;

namespace NewWorldLib.Datasheets;

public class DatasheetPropertyJsonConverter : JsonConverter<DatasheetProperty>
{
    public override DatasheetProperty? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, DatasheetProperty value, JsonSerializerOptions options)
    {
        if (value is DatasheetIntProperty intProperty)
        {
            if (intProperty.Value.HasValue)
            {
                writer.WriteNumberValue(intProperty.Value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
        else if (value is DatasheetFloatProperty floatProperty)
        {
            if (floatProperty.Value.HasValue)
            {
                writer.WriteNumberValue(floatProperty.Value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
        else if (value is DatasheetStringProperty stringProperty)
        {
            writer.WriteStringValue(stringProperty.Value);
        }
    }
}