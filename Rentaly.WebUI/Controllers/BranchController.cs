using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System.Threading.Tasks;

namespace Rentaly.WebUI.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        public async Task<IActionResult> BranchList()
        {
            var values = await _branchService.TGetListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateBranch()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBranch(Branch branch)
        {
            await _branchService.TInsertAsync(branch);
            return RedirectToAction("BranchList");
        }

        public async Task<IActionResult> DeleteBranch(int id)
        {
            await _branchService.TDeleteAsync(id);
            return RedirectToAction("BranchList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBranch(int id)
        {
            var values = await _branchService.TGetByIdAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBranch(Branch branch)
        {
            await _branchService.TUpdateAsync(branch);
            return RedirectToAction("BranchList");
        }

        public IActionResult BranchDetail(int id)
        {
            var value = _branchService.TGetBranchWithCars(id);
            return PartialView("_BranchDetailPartial", value);
        }
    }
}
