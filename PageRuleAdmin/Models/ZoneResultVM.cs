
using System.Collections.Generic;

namespace PageRuleAdmin.Models
{
    public class ZoneResultVM
    {
        public List<ResultType> Result { get; set; }

        public class ResultType
        {
            public string Id { get; set; }
            public string Type { get; set; }
        }
    }
}
