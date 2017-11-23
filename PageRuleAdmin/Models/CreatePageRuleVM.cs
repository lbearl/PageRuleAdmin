using Newtonsoft.Json;
using System.Collections.Generic;

namespace PageRuleAdmin.Models
{
    public class CreatePageRuleVM
    {
        [JsonProperty("targets")]
        public List<TargetResultType> Targets { get; set; }

        [JsonProperty("actions")]
        public List<ActionResultType> Actions { get; set; }

        [JsonProperty("status")]
        public string Status => "active";

    }
}
