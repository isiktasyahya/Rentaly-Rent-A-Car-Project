using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly IBranchService _branchService;
        private readonly IBrandService _brandService;
        private readonly ICarModelService _modelService;
        public CarController(ICarService carService, ICategoryService categoryService, IBranchService branchService, IBrandService brandService, ICarModelService modelService)
        {
            _carService = carService;
            _categoryService = categoryService;
            _branchService = branchService;
            _brandService = brandService;
            _modelService = modelService;
        }

        public async Task<IActionResult> CarList()
        {
            var values = await _carService.TGetAllCarsWithCategoryAsync();
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
            // Markalar
            var brands = await _brandService.TGetListAsync();
            ViewBag.Brands = new SelectList(brands, "BrandId", "BrandName");

            // Modeller
            var models = await _modelService.TGetListAsync();
            ViewBag.Models = new SelectList(models, "CarModelId", "ModelName");

            // Kategoriler
            var categories = await _categoryService.TGetListAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            // Şubeler
            var branches = await _branchService.TGetListAsync();
            ViewBag.Branches = new SelectList(branches, "BranchId", "BranchName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCar(Car car)
        {
            await _carService.TInsertAsync(car);
            return RedirectToAction("CarList");
        }
        // ── Güncelleme GET ──
        [HttpGet]
        public async Task<IActionResult> UpdateCar(int id)
        {
            var car = await _carService.TGetByIdAsync(id);
            if (car == null) return NotFound();

            await LoadViewBags(car.BrandId, car.CarModelId, car.CategoryId, car.BranchId);
            return View(car);
        }

        // ── Güncelleme POST ──
        [HttpPost]
        public async Task<IActionResult> UpdateCar(Car car)
        {
            await _carService.TUpdateAsync(car);
            TempData["Success"] = "Araç başarıyla güncellendi.";
            return RedirectToAction("CarList");
        }

        // ── Silme ──
        [HttpGet]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _carService.TGetByIdAsync(id);
            if (car == null) return NotFound();

            await _carService.TDeleteAsync(car.CarId);
            TempData["Success"] = "Araç başarıyla silindi.";
            return RedirectToAction("CarList");
        }
        // ── ViewBag yardımcı metot ──
        private async Task LoadViewBags(
            int selectedBrand = 0, int selectedModel = 0,
            int selectedCategory = 0, int selectedBranch = 0)
        {
            var brands = await _brandService.TGetListAsync();
            var models = await _modelService.TGetListAsync();
            var categories = await _categoryService.TGetListAsync();
            var branches = await _branchService.TGetListAsync();

            ViewBag.Brands = new SelectList(brands, "BrandId", "BrandName", selectedBrand);
            ViewBag.Models = new SelectList(models, "CarModelId", "ModelName", selectedModel);
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", selectedCategory);
            ViewBag.Branches = new SelectList(branches, "BranchId", "BranchName", selectedBranch);
        }
        // Markaya göre modelleri dönen endpoint
        [HttpGet]
        public async Task<IActionResult> GetModelsByBrand(int brandId)
        {
            var models = await _modelService.TGetListAsync();
            var filtered = models
                .Where(m => m.BrandId == brandId)
                .Select(m => new { m.CarModelId, m.ModelName });
            return Json(filtered);
        }
    }
}
