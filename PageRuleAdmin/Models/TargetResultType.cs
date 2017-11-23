using Newtonsoft.Json;

namespace PageRuleAdmin.Models
{
    public class TargetResultType
    {
        [JsonProperty("target")]
        public string Target { get; set; }
        [JsonProperty("constraint")]
        public ConstraintType Constraint { get; set; }

        public class ConstraintType
        {
            [JsonProperty("operator")]
            public string Operator { get; set; }
            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}
