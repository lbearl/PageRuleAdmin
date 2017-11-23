using PageRuleAdmin.Models;
using System.Threading.Tasks;

namespace PageRuleAdmin.Services
{
    public interface ICloudFlare
    {
        string ApiKey { get; set; }
        string UserEmail { get; set; }

        Task<bool> CreatePageRule(string targetDomain, string urlToMatch, string forwardingUrl);
        Task<PageRulesVM> GetPageRules(string domain);
        Task<bool> DeletePageRule(string domain, string id);
        Task<PageRuleVM> GetPageRuleDetail(string domain, string id);
        Task<bool> EditPageRule(string targetDomain, string urlToMatch, string forwardingUrl, string id);
    }
}
