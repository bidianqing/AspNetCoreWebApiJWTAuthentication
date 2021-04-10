
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AspNetCoreWebApiJWTAuthentication
{
    //public class LongToStringConverterWithSystemTextJson : System.Text.Json.Serialization.JsonConverter<long>
    //{
    //    public override long Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    //    {
    //        var s = reader.GetString();

    //        return long.Parse(s);
    //    }

    //    public override void Write(System.Text.Json.Utf8JsonWriter writer, long value, System.Text.Json.JsonSerializerOptions options)
    //    {
    //        writer.WriteStringValue(value.ToString());
    //    }
    //}

    public class LongToStringConverterWithNewtonsoftJson : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(long).Equals(objectType) || typeof(long?).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jt = JValue.ReadFrom(reader);

            if (jt.Type == JTokenType.Integer)
            {
                return jt.Value<long>();
            }

            if (jt.Type == JTokenType.Null && typeof(long).Equals(objectType))
            {
                throw new ArgumentException();
            }

            if (jt.Type == JTokenType.Null && typeof(long?).Equals(objectType))
            {
                return null;
            }

            if (jt.Type == JTokenType.String)
            {
                long.TryParse(jt.ToString(), out long value);

                return value;
            }


            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.ToString());
        }
    }
}
