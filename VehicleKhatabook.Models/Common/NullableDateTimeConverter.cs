﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace VehicleKhatabook.Models.Common
{
    public class NullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return null;
                }
                if (DateTime.TryParse(stringValue, out var date))
                {
                    return date;
                }
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            throw new JsonException("Invalid date format");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("o"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}