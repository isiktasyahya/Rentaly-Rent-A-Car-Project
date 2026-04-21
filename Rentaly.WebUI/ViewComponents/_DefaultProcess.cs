using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _DefaultProcess : ViewComponent
    {
        private readonly IProcessService _processService;

        public _DefaultProcess(IProcessService processService)
        {
            _processService = processService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _processService.TGetListAsync();
            return View(values);
        }
    }
}
