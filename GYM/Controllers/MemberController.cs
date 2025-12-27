using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.MemebersModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GYM.Controllers
{
    public class MemberController(IMemberService _memberService) : Controller
    {
        public IActionResult Index()
        {
            var member = _memberService.GetAllMembers();
            return View(member);
        }

        public IActionResult MemberDetails(int id)
        {
            if(id <= 0)
            {
                TempData["Error"] = "id cant be 0 or negative number";
                return RedirectToAction("Index");
            }
            var member = _memberService.GetMemberDetails(id);
            if(member is null)
            {
                TempData["Error"] = "Member not found";
                return RedirectToAction("Index");
            }
            return View(member);
        }

        public IActionResult MemberHealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "id cant be 0 or negative number";
                return RedirectToAction("Index");
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["Error"] = "Member not found";
                return RedirectToAction("Index");
            }

            var HR = _memberService.GetMemberHealthRecordDetails(id);
            return View(HR);
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberModelView createMemberModelView)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check data and missing fields");
                return View(nameof(Create), createMemberModelView);
            }
            var flag = await  _memberService.CreateMember(createMemberModelView);
            if(flag)
            {
                TempData["Message"] = "Member Created Successfully";
            }else
            {
                TempData["ErrorMessage"] = "Member creation failed";
            }

                return RedirectToAction("Index");
            

        }

        public async Task<IActionResult> MemberEdit(int id)
        {
            if(id <= 0)
            {
                TempData["Error"] = "Id cant be 0 or negative number";
                return RedirectToAction("Index");
            }
            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["Error"] = "Member is not found";
                return RedirectToAction("Index");
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> MemberEdit([FromRoute]int id,MemberToUpdateViewModel memberModel)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check data and missing fields");
                return View(nameof(MemberEdit), memberModel);
            }
            else
            {
                var flag = await _memberService.UpdateMember(id, memberModel);
                if (flag)
                {
                    TempData["Message"] = "Member Updated Successfully";
                }
                else
                {
                    TempData["Error"] = "Member Update failed";
                }
                return RedirectToAction("Index");
            }                
        }

        public IActionResult DeleteMember(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Id cant be 0 or negative number";
                return RedirectToAction("Index");
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["Error"] = "Member is not found";
                return RedirectToAction("Index");
            }
            return View(member);
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var flag = await _memberService.RemoveMember(id);
            if (flag)
            {
                TempData["Message"] = "Member is deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = "Member is not deleted ";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
