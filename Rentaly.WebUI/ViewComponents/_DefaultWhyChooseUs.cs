using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _DefaultWhyChooseUs : ViewComponent
    {
        private readonly IWhyChooseUsService _whyChooseUs;

        public _DefaultWhyChooseUs(IWhyChooseUsService whyChooseUs)
        {
            _whyChooseUs = whyChooseUs;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _whyChooseUs.TGetListAsync();
            return View(values);
        }
    }
}
