using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.BookingModelViews;
using GYM.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYM.Controllers
{
    public class BookingController(IBookingService _bookingService) : Controller
    {
        public IActionResult Index()
        {
            var sessions = _bookingService.GetAllSessionsWithTrainerAndCategory();
            return View(sessions);
        }

        public IActionResult GetAllMembersForUpcomingSession(int id)
        {
            var members = _bookingService.GetAllMembersBySessionId(id);
            return View(members);
        }
        public IActionResult GetAllMembersForOngoingSession(int id)
        {
            var members = _bookingService.GetAllMembersBySessionId(id);
            return View(members);
        }

        public IActionResult Create(int id)
        {
            var members = _bookingService.GetMembersForDropdown(id);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model)
        {
            var result = await _bookingService.Create(model);
            if (result)
            {
                TempData["Message"] = "Booking Created successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to Create Booking.";
            }

            return RedirectToAction(nameof(GetAllMembersForUpcomingSession), new { id = model.SessionId });
        }

        public IActionResult CancelBooking(int memberId, int sessionId)
        {
            return View(new MemberForSessionViewModel
            {
                MemberId = memberId,
                SessionId = sessionId
            });
        }


        [HttpPost]
        public async Task<IActionResult> Cancel(MemberForSessionViewModel model)
        {
            var result = await _bookingService.Cancel(model);
            if (result)
            {
                TempData["Message"] = "Booking canceled successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to cancel Booking.";
            }
            return RedirectToAction(nameof(GetAllMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Attended(MemberForSessionViewModel model)
        {
            var result = await _bookingService.Attended(model);

            if (result)
                TempData["SuccessMessage"] = "Member attended successfully";
            else
                TempData["ErrorMessage"] = "Member attendance can't be marked";

            return RedirectToAction(nameof(GetAllMembersForOngoingSession), new { id = model.SessionId });
        }
    }
}
