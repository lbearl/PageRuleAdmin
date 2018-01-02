using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PageRuleAdmin.Models
{
    public class ActionResultType
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("value")]
        public ValueResultType Value { get; set; }

        public class ValueResultType
        {
            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty(PropertyName = "status_code")]
            public int StatusCode { get; set; }

            [OnError]
            internal void OnError(StreamingContext context, ErrorContext errorContext)
            {
                errorContext.Handled = true;
            }
        }

        [OnError]
        internal void OnError(StreamingContext context, ErrorContext errorContext)
        {
            errorContext.Handled = true;
        }
    }
}
