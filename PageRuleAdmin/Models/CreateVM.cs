using System.ComponentModel.DataAnnotations;

namespace PageRuleAdmin.Models
{
    public class CreateVM
    {
        [Required]
        public string Domain { get; set; }

        [Required]
        public string MatchUrlBlob { get; set; }

        [Required, DataType(DataType.Url)]
        public string ForwardingUrl { get; set; }
    }
}
