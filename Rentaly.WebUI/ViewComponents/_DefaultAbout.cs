using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _DefaultAbout : ViewComponent
    {
        private readonly IAboutService _aboutService;
        private readonly IRentalService _rentalService;
        private readonly ICustomerService _customerService;
        private readonly ICarService _carService;
        public _DefaultAbout(IAboutService aboutService, IRentalService rentalService, ICustomerService customerService, ICarService carService)
        {
            _aboutService = aboutService;
            _rentalService = rentalService;
            _customerService = customerService;
            _carService = carService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.rentalCount = await _rentalService.TGetRentalCountAsync();
            ViewBag.customerCount = await _customerService.TGetCustomerCountAsync();
            ViewBag.carCount = await _carService.TGetCarCountAync();
            ViewBag.year = DateTime.Now.Year - 2010;

            var values = await _aboutService.TGetListAsync();
            return View(values);

           
        }

    }
}
