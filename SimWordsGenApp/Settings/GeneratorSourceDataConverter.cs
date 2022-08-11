using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SimWordsGenApp.SettingsConverters
{
    public class GeneratorSourceDataConverter : JsonConverter<Dictionary<uint, IReadOnlyDictionary<char, int>>>
    {
        public static GeneratorSourceDataConverter Default { get; } = new GeneratorSourceDataConverter();

        public override Dictionary<uint, IReadOnlyDictionary<char, int>> ReadJson(JsonReader reader, Type objectType, Dictionary<uint, IReadOnlyDictionary<char, int>> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var result = new Dictionary<uint, IReadOnlyDictionary<char, int>>();
            if (reader.TokenType != JsonToken.StartObject) throw new JsonSerializationException("Expected start object");
            reader.Read();
            while (reader.TokenType != JsonToken.EndObject)
            {
                if (reader.TokenType != JsonToken.PropertyName) throw new JsonSerializationException("Expected property name");
                var key = reader.Value as string;
                var keyValue = uint.Parse(key);

                IReadOnlyDictionary<char, int> data;
                if (!result.TryGetValue(keyValue, out data))
                {
                    data = new Dictionary<char, int>();
                    result[keyValue] = data;
                }

                reader.Read();
                if (reader.TokenType != JsonToken.StartObject) throw new JsonSerializationException("Expected start object");
                reader.Read();
                while (reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType != JsonToken.PropertyName) throw new JsonSerializationException("Expected property name");
                    var key2 = reader.Value as string;
                    var key2Value = (char)long.Parse(key2);
                    reader.Read();
                    if (reader.TokenType != JsonToken.Integer) throw new JsonSerializationException("Expected integer");
                    var value = reader.Value as long?;
                    (data as Dictionary<char, int>).Add(key2Value, (int)value.Value);
                    reader.Read();
                }
                reader.Read();
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, Dictionary<uint, IReadOnlyDictionary<char, int>> value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var pair in value)
                if (pair.Value?.Count > 0)
                {
                    writer.WritePropertyName(pair.Key.ToString());
                    writer.WriteStartObject();
                    foreach (var pair2 in pair.Value)
                    {
                        writer.WritePropertyName(((ushort)pair2.Key).ToString());
                        writer.WriteValue(pair2.Value);
                    }
                    writer.WriteEndObject();
                }
            writer.WriteEndObject();
        }
    }
}
