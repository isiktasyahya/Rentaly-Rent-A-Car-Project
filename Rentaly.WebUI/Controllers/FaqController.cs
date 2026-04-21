using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System.Threading.Tasks;

namespace Rentaly.WebUI.Controllers
{
    public class FaqController : Controller
    {
        private readonly IFaqService _faqService;

        public FaqController(IFaqService faqService)
        {
            _faqService = faqService;
        }

        public async Task<IActionResult> FaqList()
        {
            var values = await _faqService.TGetListAsync();
            return View(values);
        }

        public async Task<IActionResult> DeleteFaq(int id)
        {
            await _faqService.TDeleteAsync(id);
            return RedirectToAction("FaqList");
        }

        [HttpGet]
        public IActionResult CreateFaq()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateFaq(Faq faq)
        {
            await _faqService.TInsertAsync(faq);
            return RedirectToAction("FaqList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFaq(int id)
        {
            var values = await _faqService.TGetByIdAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFaq(Faq faq)
        {
            await _faqService.TUpdateAsync(faq);
            return RedirectToAction("FaqList");

        }



    }
}
