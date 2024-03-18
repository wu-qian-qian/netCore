using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Commons
{
    public static class JsonExtentions
    {
        //如果不设置这个，那么"雅思真题"就会保存为"\u96C5\u601D\u771F\u9898"
        public readonly static JavaScriptEncoder Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

        public static JsonSerializerOptions CreateJsonSerializerOptions(bool camelCase = false)
        {
            JsonSerializerOptions opt = new JsonSerializerOptions { Encoder = Encoder };
            if (camelCase)
            {
                opt.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                opt.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            }
            opt.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
            return opt;
        }

        public static string ToJsonString(this object value, bool camelCase = false)
        {
            var opt = CreateJsonSerializerOptions(camelCase);
            return JsonSerializer.Serialize(value, value.GetType(), opt);
        }

        public static T? ParseJson<T>(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default(T);
            }
            var opt = CreateJsonSerializerOptions();
            return JsonSerializer.Deserialize<T>(value, opt);
        }
    }
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        private readonly string _dateFormatString;
        public DateTimeJsonConverter()
        {
            _dateFormatString = "yyyy-MM-dd HH:mm:ss";
        }

        public DateTimeJsonConverter(string dateFormatString)
        {
            _dateFormatString = dateFormatString;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? str = reader.GetString();
            if (str == null)
            {
                return default(DateTime);
            }
            else
            {
                return DateTime.Parse(str);
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            //固定用服务器所在的时区，前端如果想适应用户的时区，请自己调整
            writer.WriteStringValue(value.ToString(_dateFormatString));
        }
    }
}
