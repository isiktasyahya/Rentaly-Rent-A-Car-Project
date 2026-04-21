using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _DefaultCarList : ViewComponent
    {
        private readonly ICarService _carService;

        public _DefaultCarList(ICarService carService)
        {
            _carService = carService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _carService.TGetLast10CarsAsync();
            return View(values);
        }



    }
}
