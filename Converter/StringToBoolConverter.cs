using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class StringToBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string stringValue = reader.GetString();

        if (bool.TryParse(stringValue, out bool result))
        {
            return result;
        }

        return false; // If the parsing fails, return false by default
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
