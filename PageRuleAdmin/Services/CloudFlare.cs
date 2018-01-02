using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PageRuleAdmin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PageRuleAdmin.Services
{
    public class CloudFlare : ICloudFlare
    {
        private const string API_ENDPOINT = "https://api.cloudflare.com/client/v4/";
        public string ApiKey { get; set; }
        public string UserEmail { get; set; }

        public async Task<PageRulesVM> GetPageRules(string domain)
        {
            var zoneId = await ResolveZone(domain);
            var rules = await PerformWebRequest<PageRulesVM>($"{API_ENDPOINT}/zones/{zoneId}/pagerules", HttpMethod.Get);
            rules.Result = rules.Result.Where(x => x.Actions.Any(y => y.Id == "forwarding_url")).ToList();
            return rules;
        }

        public async Task<bool> CreatePageRule(string targetDomain, string urlToMatch, string forwardingUrl)
        {
            var zoneId = await ResolveZone(targetDomain);
            var createVm = new CreatePageRuleVM
            {
                Actions = new List<ActionResultType>
                {
                    new ActionResultType
                    {
                        Id = "forwarding_url",
                        Value = new ActionResultType.ValueResultType
                        {
                            Url = forwardingUrl,
                            StatusCode = 301
                        }
                    }
                },
                Targets = new List<TargetResultType>
                {
                    new TargetResultType
                    {
                        Constraint = new TargetResultType.ConstraintType
                        {
                            Operator = "matches",
                            Value = urlToMatch
                        },
                        Target = "url"
                    }
                }
            };

            // The replacement here is hacky, but I can't get Json.NET to do it the "right" way.
            var json = JsonConvert.SerializeObject(createVm).Replace("/", "\\/");

            var response = await PerformWebRequest<ApiResponseVM>($"{API_ENDPOINT}/zones/{zoneId}/pagerules", HttpMethod.Post, json);
            return response.Success;
        }

        public async Task<bool> DeletePageRule(string domain, string id)
        {
            var zoneId = await ResolveZone(domain);
            var result = await PerformWebRequest<ApiResponseVM>($"{API_ENDPOINT}/zones/{zoneId}/pagerules/{id}", HttpMethod.Delete);

            return result.Success;
        }

        public async Task<PageRuleVM> GetPageRuleDetail(string domain, string id)
        {
            var zoneId = await ResolveZone(domain);
            return await PerformWebRequest<PageRuleVM>($"{API_ENDPOINT}/zones/{zoneId}/pagerules/{id}", HttpMethod.Get);
        }

        public async Task<bool> EditPageRule(string targetDomain, string urlToMatch, string forwardingUrl, string id)
        {
            var zoneId = await ResolveZone(targetDomain);
            var editVm = new CreatePageRuleVM
            {
                Actions = new List<ActionResultType>
                {
                    new ActionResultType
                    {
                        Id = "forwarding_url",
                        Value = new ActionResultType.ValueResultType
                        {
                            Url = forwardingUrl,
                            StatusCode = 301
                        }
                    }
                },
                Targets = new List<TargetResultType>
                {
                    new TargetResultType
                    {
                        Constraint = new TargetResultType.ConstraintType
                        {
                            Operator = "matches",
                            Value = urlToMatch
                        },
                        Target = "url"
                    }
                }
            };

            var json = JsonConvert.SerializeObject(editVm).Replace("/", "\\/");

            var result = await PerformWebRequest<ApiResponseVM>($"{API_ENDPOINT}/zones/{zoneId}/pagerules/{id}", new HttpMethod("PATCH"), json);

            return result.Success;
        }

        private async Task<string> ResolveZone(string domain)
        {
            var result = await PerformWebRequest<ZoneResultVM>($"{API_ENDPOINT}/zones?name={domain}", HttpMethod.Get);
            return result.Result.First().Id;
        }

        private async Task<T> PerformWebRequest<T>(string uri, HttpMethod method, string postBody = null)
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                RequestUri = new System.Uri(uri),
                Method = method,
            };

            if(postBody != null)
            {
                request.Content = new StringContent(postBody, Encoding.UTF8, "application/json");
            }

            request.Headers.Add("X-Auth-Email", UserEmail);
            request.Headers.Add("X-Auth-Key", ApiKey);

            var result = await client.SendAsync(request);

            var resultString = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(resultString);
        }
    }
}
