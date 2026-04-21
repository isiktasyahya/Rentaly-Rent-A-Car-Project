using Microsoft.AspNetCore.Mvc;

namespace Rentaly.WebUI.ViewComponents
{
    public class _DefaultContact:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
