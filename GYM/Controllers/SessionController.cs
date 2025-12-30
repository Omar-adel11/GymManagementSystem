using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.SessionsModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYM.Controllers
{
    [Authorize]
    public class SessionController(ISessionService _sessionService) : Controller
    {
        
            public IActionResult Index()
            {
                var sessions = _sessionService.GetAllSessions();
                return View(sessions);
            }

            public IActionResult Create()
            {
                LoadCategoriesDropdown();
                LoadTrainersDropdown();
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(CreateSessionModelView input)
            {
                if (!ModelState.IsValid)
                {
                    LoadCategoriesDropdown();
                    LoadTrainersDropdown();
                    return View(input);
                }

                var result = await _sessionService.CreateSession(input);
                if (result)
                {
                    TempData["SuccessMessage"] = "Session Created Successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Falied to create session!";
                    return View(input);
                }
            }

            public IActionResult Details(int id)
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid session id!";
                    return RedirectToAction(nameof(Index));
                }

                var session = _sessionService.GetSessionById(id);

                if (session is null)
                {
                    TempData["ErrorMessage"] = "Session not found!";
                    return RedirectToAction(nameof(Index));
                }

                return View(session);
            }

            public IActionResult Edit(int id)
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid session id!";
                    return RedirectToAction(nameof(Index));
                }

                var session = _sessionService.GetSessionToUpdate(id);

                if (session is null)
                {
                    TempData["ErrorMessage"] = "Session can not be updated!";
                    return RedirectToAction(nameof(Index));
                }

                LoadTrainersDropdown();
                return View(session);
            }

            [HttpPost]
            public async Task<IActionResult> Edit([FromRoute] int id, UpdateSession input)
            {
                if (!ModelState.IsValid)
                {
                    LoadTrainersDropdown();
                    return View(input);
                }

                var result = await _sessionService.UpdateSession(id, input);

                if (result)
                    TempData["SuccessMessage"] = "Session updated successfully.";
                else
                    TempData["ErrorMessage"] = "Failed to update session!";

                return RedirectToAction(nameof(Index));
            }
            public IActionResult Delete([FromRoute] int id)
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid session id!";
                    return RedirectToAction(nameof(Index));
                }

                var session = _sessionService.GetSessionById(id);

                if (session is null)
                {
                    TempData["ErrorMessage"] = "Session not found!!";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.SessionId = id;
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> DeleteConfirmed([FromForm] int id)
            {
                var result = await _sessionService.RemoveSession(id);

                if (result)
                    TempData["SuccessMessage"] = "Session deleted successfully.";
                else
                    TempData["ErrorMessage"] = "Failed to delete session!";

                return RedirectToAction(nameof(Index));
            }


            #region Helper Methods
            public void LoadCategoriesDropdown()
            {
                var categories = _sessionService.GetCategoriesDropdown();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
            }
            public void LoadTrainersDropdown()
            {
                var trainers = _sessionService.GetTrainersDropdown();
                ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
            }
            #endregion
        
    }
}
