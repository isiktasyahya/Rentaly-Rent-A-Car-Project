using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using X.PagedList;
using X.PagedList.Extensions;

namespace Rentaly.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBranchService _branchService;

        public DefaultController(ICarService carService, IBranchService branchService)
        {
            _carService = carService;
            _branchService = branchService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CarList(
            int? branchId = null,
            int? categoryId = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            DateTime? pickupDate = null,
            DateTime? returnDate = null,
            string? pickupTime = null,
            string? returnTime = null,
            int? pickupBranchId = null,
            string? sort = null,
            int page = 1)
        {
            // Ana sayfa formundan saat bilgisi geliyorsa tarihe ekle
            if (pickupDate.HasValue && !string.IsNullOrEmpty(pickupTime))
                pickupDate = DateTime.Parse(pickupDate.Value.ToString("yyyy-MM-dd") + " " + pickupTime);

            if (returnDate.HasValue && !string.IsNullOrEmpty(returnTime))
                returnDate = DateTime.Parse(returnDate.Value.ToString("yyyy-MM-dd") + " " + returnTime);

            // Ana sayfa aramasından pickupBranchId geliyorsa branchId olarak kullan
            if (pickupBranchId.HasValue && !branchId.HasValue)
                branchId = pickupBranchId;

            List<Rentaly.EntityLayer.Entities.Car> values;

            // Ana sayfadan arama geliyorsa TSearchAvailableCarsAsync kullan
            if (pickupBranchId.HasValue || (categoryId.HasValue && pickupDate.HasValue))
            {
                values = await _carService.TSearchAvailableCarsAsync(
                    categoryId,
                    pickupBranchId ?? branchId,
                    pickupDate,
                    returnDate);
            }
            else
            {
                // Normal filtre (CarList sayfasındaki filtreler)
                values = await _carService.TGetFilteredCarsAsync(
                    branchId,
                    categoryId,
                    minPrice,
                    maxPrice,
                    pickupDate,
                    returnDate,
                    sort);
            }

            // BrandId filtresi
            if (brandId.HasValue)
                values = values.Where(x => x.BrandId == brandId).ToList();

            ViewBag.TotalCount = values.Count;

            return View(values.ToPagedList(page, 10));
        }

        // ── Araç Detay ──
        public async Task<IActionResult> CarDetail(int id)
        {
            var car = await _carService.TGetCarDetailAsync(id);
            if (car == null) return NotFound();

            var branches = await _branchService.TGetListAsync();
            ViewBag.Branches = new SelectList(branches, "BranchId", "BranchName");

            return View(car);
        }
    }
}