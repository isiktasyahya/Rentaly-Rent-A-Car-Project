using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _DefaultFaq : ViewComponent
    {
        private readonly IFaqService _faqService;

        public _DefaultFaq(IFaqService faqService)
        {
            _faqService = faqService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _faqService.TGetListAsync();
            return View(values);
        }
    }
}
