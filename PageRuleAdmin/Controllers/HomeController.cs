using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PageRuleAdmin.Models;
using Microsoft.Extensions.Configuration;
using PageRuleAdmin.Services;
using System.Threading.Tasks;
using System.Linq;

namespace PageRuleAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICloudFlare _cloudFlare;

        public HomeController(IConfiguration configuration, ICloudFlare cloudFlare)
        {
            _configuration = configuration;
            _cloudFlare = cloudFlare;
            _cloudFlare.ApiKey = _configuration["CF_API_KEY"];
            _cloudFlare.UserEmail = _configuration["CF_EMAIL"];
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetRules(string domain)
        {

            var result = await _cloudFlare.GetPageRules(domain);
            result.Domain = domain;
            return View(result);

        }

        public IActionResult Create(string domain)
        {
            var vm = new CreateVM
            {
                Domain = domain
            };
            return View(nameof(Create), vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVM createVm)
        {
            var result = await _cloudFlare.CreatePageRule(createVm.Domain, createVm.MatchUrlBlob, createVm.ForwardingUrl);

            if (result)
            {
                TempData["Message"] = "Successfully created rule matching: " + createVm.MatchUrlBlob;
                return RedirectToAction(nameof(GetRules), new { domain = createVm.Domain });
            }
            throw new System.Exception("Failed to create");
        }

        public async Task<IActionResult> Edit(string id, string domain)
        {
            var details = await _cloudFlare.GetPageRuleDetail(domain, id);
            var vm = new EditVM
            {
                Domain = domain,
                ForwardingUrl = details.Result.Actions.First().Value.Url,
                MatchUrlBlob = details.Result.Targets.First().Constraint.Value,
                Id = id
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditVM editVM)
        {
            var result = await _cloudFlare.EditPageRule(editVM.Domain, editVM.MatchUrlBlob, editVM.ForwardingUrl, editVM.Id);

            if (result)
            {
                TempData["Message"] = "Successfully edited rule matching: " + editVM.MatchUrlBlob;
                return RedirectToAction(nameof(GetRules), new { domain = editVM.Domain });
            }
            throw new System.Exception("Failed to create");
        }


        public async Task<IActionResult> Delete(string id, string domain)
        {
            var result = await _cloudFlare.DeletePageRule(domain, id);

            if (result)
            {
                TempData["Message"] = "Successfully deleted rule with id: " + id;
                return RedirectToAction(nameof(GetRules), new { domain = domain });
            }
            throw new System.Exception("Failed to create");
        }
    }
}
