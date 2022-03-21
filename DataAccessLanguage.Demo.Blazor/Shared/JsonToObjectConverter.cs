using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLanguage.Demo.Blazor
{
	public class JsonToObjectConverter : JsonConverterFactory
	{
		private static JsonConverter<object> _valueConverter = null;

		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert == typeof(object);
		}

		public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
		{
			return _valueConverter ?? (_valueConverter = new DictionaryConverterInner());
		}

		private class DictionaryConverterInner : JsonConverter<object>
		{
			public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.StartObject)
					return getValue(ref reader, options);
				else
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();

					while (reader.Read())
					{
						if (reader.TokenType == JsonTokenType.EndObject)
							return dictionary;

						if (reader.TokenType != JsonTokenType.PropertyName)
							throw new JsonException();

						string propertyName = reader.GetString();
						reader.Read();

						dictionary[propertyName] = getValue(ref reader, options);
					}
					return dictionary;
				}
			}

			private object getValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.String:
						string s = reader.GetString();
						if (s.Contains(':') && TimeSpan.TryParse(s, out TimeSpan time))
							return time;
						else if (s.Contains(':') && DateTime.TryParse(s, out DateTime res))
							return res;
						return reader.GetString();
					case JsonTokenType.False:
						return false;
					case JsonTokenType.True:
						return true;
					case JsonTokenType.Null:
						return null;
					case JsonTokenType.Number:
						if (reader.TryGetInt64(out long _long))
							return _long;
						else if (reader.TryGetDecimal(out decimal _dec))
							return _dec;
						throw new JsonException($"Unhandled Number value");
					case JsonTokenType.StartObject:
						return JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);
					case JsonTokenType.StartArray:
						List<object> array = new List<object>();
						while (reader.Read() &&
							reader.TokenType != JsonTokenType.EndArray)
						{
							array.Add(getValue(ref reader, options));
						}
						return array;
				}
				throw new JsonException($"Unhandled TokenType {reader.TokenType}");
			}

			public override void Write(Utf8JsonWriter writer, object hashtable, JsonSerializerOptions options)
			{
				JsonSerializer.Serialize(writer, hashtable, options);
			}
		}
	}
}