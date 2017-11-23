using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PageRuleAdmin.Models
{
    public class PageRuleVM
    {
        public bool Success { get; set; }
        public string Domain { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Messages { get; set; }

        public ResultType Result { get; set; }

        public class ResultType
        {
            public string Id { get; set; }
            public List<TargetResultType> Targets { get; set; }
            public List<ActionResultType> Actions { get; set; }
            public int Priority { get; set; }
            public string Status { get; set; }
            [JsonProperty(PropertyName = "modified_on")]
            public DateTime ModifiedOn { get; set; }
            [JsonProperty(PropertyName = "created_on")]
            public DateTime CreatedOn { get; set; }


        }
    }
}
