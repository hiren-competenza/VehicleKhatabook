using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleKhatabook.Models.Common
{

    public class NullableBoolConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return null;
                }
                if (bool.TryParse(stringValue, out var boolean))
                {
                    return boolean;
                }
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            throw new JsonException("Invalid boolean format");
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteBooleanValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
