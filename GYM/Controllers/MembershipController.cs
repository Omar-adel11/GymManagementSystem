using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.MembershipsViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYM.Controllers
{
    public class MembershipController(IMembershipService _membershipService) : Controller
    {
        public IActionResult Index()
        {
            var memberships = _membershipService.GetAllMemberships();
            return View(memberships);
        }

        public IActionResult Create()
        {
            LoadDropDowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipModelView model)
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "check missing fields";
                LoadDropDowns();
                return View(model);
            }

            var result = await _membershipService.CreateMembership(model);
            if (result)
            {
                TempData["Message"] = "Membership created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Membership can not be created";
                return RedirectToAction("Index");

            }

            
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _membershipService.DeleteMembership(id);
            if (result)
            {
                TempData["Message"] = "Membership Deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Membership can not be Deleted";
                return RedirectToAction("Index");

            }
        }


        private void LoadDropDowns()
        {
            ViewBag.plans = new SelectList(_membershipService.GetPlansForDropDown(), "Id", "Name");
            ViewBag.members = new SelectList(_membershipService.GetMembersForDropDown(), "Id", "Name");
        }
    }
}
