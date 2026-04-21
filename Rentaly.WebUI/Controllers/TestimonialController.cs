using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System.Threading.Tasks;

namespace Rentaly.WebUI.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly ITestiominalService _testiominalService;

        public TestimonialController(ITestiominalService testiominalService)
        {
            _testiominalService = testiominalService;
        }

        public async Task<IActionResult> TestimonialList()
        {
            var values = await _testiominalService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateTestimonial()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTestimonial(Testimonial testimonial)
        {
            await _testiominalService.TInsertAsync(testimonial);
            return RedirectToAction("TestimonialList");
        }

        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            await _testiominalService.TDeleteAsync(id);
            return RedirectToAction("TestimonialList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTestimonial(int id)
        {
            var value = await _testiominalService.TGetByIdAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTestimonial(Testimonial testimonial)
        {
            await _testiominalService.TUpdateAsync(testimonial);
            return RedirectToAction("TestimonialList");
        }
    }
}
