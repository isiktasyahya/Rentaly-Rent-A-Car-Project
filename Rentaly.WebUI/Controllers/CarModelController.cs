using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.Controllers
{
    public class CarModelController : Controller
    {
        private readonly ICarModelService _carModelService;

        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        public async Task<IActionResult> CarModelList(string search = "")
        {
            var values = await _carModelService.TGetCarsGroupedByModelAsync();

            if (!string.IsNullOrEmpty(search))
                values = values.Where(x =>
                    x.ModelName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    x.Brand.BrandName.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            ViewBag.Search = search;
            ViewBag.TotalModels = values.Count;
            ViewBag.TotalCars = values.Sum(x => x.Cars?.Count ?? 0);
            ViewBag.TotalBrands = values.Select(x => x.BrandId).Distinct().Count();

            return View(values);
        }
    }
}
