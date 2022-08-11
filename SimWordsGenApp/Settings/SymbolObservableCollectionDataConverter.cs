using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SimWordsGenApp.SettingsConverters
{
    public class SymbolObservableCollectionDataConverter : JsonConverter<ObservableCollection<Symbol>>
    {
        public static SymbolObservableCollectionDataConverter Default { get; } = new SymbolObservableCollectionDataConverter();

        public override ObservableCollection<Symbol> ReadJson(JsonReader reader, Type objectType, ObservableCollection<Symbol> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray) throw new JsonSerializationException("Expected start array");
            reader.Read();
            if (reader.TokenType == JsonToken.EndArray)
                return null;
            if (reader.TokenType != JsonToken.String) throw new JsonSerializationException("Expected string value");
            var origin = (reader.Value as string).ToCharArray();
            reader.Read();
            if (reader.TokenType != JsonToken.String) throw new JsonSerializationException("Expected string value");
            var replace = (reader.Value as string).ToCharArray();
            reader.Read();
            if (reader.TokenType != JsonToken.EndArray) throw new JsonSerializationException("Expected start array");
            return new ObservableCollection<Symbol>(origin.Glue(replace, (o, r) => new Symbol(o, r)));
        }

        public override void WriteJson(JsonWriter writer, ObservableCollection<Symbol> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            var sb = new StringBuilder();
            foreach (var item in value)
                sb.Append(item.Origin);
            writer.WriteValue(sb.ToString());

            sb = new StringBuilder();
            foreach (var item in value)
                sb.Append(item.Replace);
            writer.WriteValue(sb.ToString());
            writer.WriteEndArray();
        }
    }
}
