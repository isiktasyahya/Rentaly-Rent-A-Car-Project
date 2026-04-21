using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System.Threading.Tasks;

namespace Rentaly.WebUI.Controllers
{
    public class WhyChooseUsController : Controller
    {
        private readonly IWhyChooseUsService _whyChooseUsService;

        public WhyChooseUsController(IWhyChooseUsService whyChooseUsService)
        {
            _whyChooseUsService = whyChooseUsService;
        }

        public async Task<IActionResult> WhyChooseUsList()
        {
            var values = await _whyChooseUsService.TGetListAsync();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateWhyChooseUs()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateWhyChooseUs(WhyChooseUs whyChooseUs)
        {
            await _whyChooseUsService.TInsertAsync(whyChooseUs);
            return RedirectToAction("WhyChooseUsList");
        }

        public async Task<IActionResult> DeleteWhyChooseUs(int id)
        {
            await _whyChooseUsService.TDeleteAsync(id);
            return RedirectToAction("WhyChooseUsList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateWhyChooseUs(int id)
        {
            var values = await _whyChooseUsService.TGetByIdAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateWhyChooseUs(WhyChooseUs whyChooseUs)
        {
            await _whyChooseUsService.TUpdateAsync(whyChooseUs);
            return RedirectToAction("WhyChooseUsList");
        }

    }
}
