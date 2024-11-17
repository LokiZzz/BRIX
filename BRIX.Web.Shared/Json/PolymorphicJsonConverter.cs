using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;

namespace BRIX.Web.Shared.Json
{
    public class PolymorphicJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            return true;
        }

        public override JsonConverter CreateConverter(
            Type type,
            JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(PolymorphicJsonConverter<>).MakeGenericType(type),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;

            return converter;
        }

        private class PolymorphicJsonConverter<TType> : JsonConverter<TType>
        {
            private const string CLRType = nameof(CLRType);

            public override TType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }

                reader.Read();
                Type type = typeToConvert;

                if(reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == CLRType)
                {
                    reader.Read();
                    string typeName = reader.GetString()
                        ?? throw new InvalidDataException("Не распознана строка с CLR типом.");
                    type = Assembly.GetExecutingAssembly().GetType(typeName)
                        ?? throw new InvalidDataException("Не найден CLR тип."); ;
                }

                JsonConverter<TType> defaultConverter = (JsonConverter<TType>)options.GetConverter(type);
                TType value = defaultConverter.Read(ref reader, type, options) ?? default!;

                return value;
            }

            public override void Write(Utf8JsonWriter writer, TType value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(CLRType);
                writer.WriteStringValue(typeof(TType).FullName);
                JsonSerializer.Serialize(writer, value);

                writer.WriteEndObject();
            }
        }
    }
}
