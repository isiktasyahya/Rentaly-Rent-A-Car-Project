using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _CarFilter : ViewComponent
    {
        private readonly IBranchService _branchService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public _CarFilter(
            IBranchService branchService,
            ICategoryService categoryService,
            IBrandService brandService)
        {
            _branchService = branchService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // ── Dropdown verileri ──
            ViewBag.Branches = await _branchService.TGetListAsync();
            ViewBag.Categories = await _categoryService.TGetListAsync();
            ViewBag.Brands = await _brandService.TGetListAsync();

            // ── Aktif filtre değerleri URL'den okunur ──
            var q = HttpContext.Request.Query;

            ViewBag.CurrentBranchId = q["branchId"].ToString();
            ViewBag.CurrentCategoryId = q["categoryId"].ToString();
            ViewBag.CurrentBrandId = q["brandId"].ToString();
            ViewBag.CurrentMinPrice = q["minPrice"].ToString() is { Length: > 0 } min ? min : "0";
            ViewBag.CurrentMaxPrice = q["maxPrice"].ToString() is { Length: > 0 } max ? max : "10000";
            ViewBag.CurrentPickupDate = q["pickupDate"].ToString();
            ViewBag.CurrentReturnDate = q["returnDate"].ToString();

            return View();
        }
    }
}