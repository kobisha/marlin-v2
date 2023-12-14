using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class NullableDecimalConverter : JsonConverter<decimal?>
{
    public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType == JsonTokenType.String && reader.GetString() == string.Empty)
            return null;

        // Use a custom NumberStyles to allow handling values with different decimal separators, etc.
        if (decimal.TryParse(reader.GetString(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
            return result;

        throw new JsonException($"Cannot convert the value '{reader.GetString()}' to a decimal.");
    }

    public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            writer.WriteNumberValue(value.Value);
    }
}
