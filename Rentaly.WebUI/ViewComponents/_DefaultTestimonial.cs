using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _DefaultTestimonial : ViewComponent
    {
        private readonly ITestiominalService _testiominalService;

        public _DefaultTestimonial(ITestiominalService testiominalService)
        {
            _testiominalService = testiominalService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _testiominalService.TGetListAsync();
            return View(values);
        }
    }
}
