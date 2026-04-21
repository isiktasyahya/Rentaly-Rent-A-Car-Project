using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;

namespace Rentaly.WebUI.ViewComponents
{
    public class _DefaultCategories : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public _DefaultCategories(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _categoryService.TGetListAsync();
            return View(values);
        }
    }
}
