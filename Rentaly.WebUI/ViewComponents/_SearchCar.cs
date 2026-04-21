using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _SearchCar:ViewComponent
    {
        private readonly IBranchService _branchService;
        private readonly ICategoryService _categoryService;
        public _SearchCar(IBranchService branchService, ICategoryService categoryService)
        {
            _branchService = branchService;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.branches = await _branchService.TGetListAsync();
            ViewBag.categories = await _categoryService.TGetListAsync();

            return View();
        }
    }
}
