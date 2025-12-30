using GYM.BLL.ModelViews.PlansModelViews;
using GYM.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    [Authorize]
    public class PlanController(IPlanService _planService) : Controller
    {
        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }

        public IActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["Error"] = "id cant be zero or negative number";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if(plan == null)
            {
                TempData["Error"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        public IActionResult Edit(int id)
        {
            if(id <= 0)
            {
                TempData["Error"] = "id cant be zero or negative number";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if(plan == null)
            {
                TempData["Error"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, PlanToUpdateModelView model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid","Check data and missing fields");
                return View(model);
            }
            var isUpdated =  await _planService.UpdatePlan(id, model);
            if(!isUpdated)
            {
                TempData["Error"] = "Failed to update plan";
                return RedirectToAction(nameof(Index));
            }
            TempData["Message"] = "Plan updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id)
        {
            var plan = await _planService.ToggleStatus(id);
            if(!plan)
            {
                TempData["Error"] = "Failed to change plan status";     
            }
            else
            {
                TempData["Message"] = "Plan status changed successfully";
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
