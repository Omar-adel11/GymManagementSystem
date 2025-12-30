using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.TrainersModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController(ITrainerService _trainerService) : Controller
    {
        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        public IActionResult TrainerDetails(int id)
        {
            if(id <= 0)
            {
                TempData["Error"] = "Id cant be zero or negative number";
                return RedirectToAction("Index");
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if(trainer == null)
            {
                TempData["Error"] = "Trainer not found";
                return RedirectToAction("Index");
            }

            return View(trainer);
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Id cant be zero or negative number";
                return RedirectToAction("Index");
            }
            var trainer = _trainerService.GetTrainerToBeUpdated(id);
            if (trainer == null)
            {
                TempData["Error"] = "Trainer not found";
                return RedirectToAction("Index");
            }
            return View(trainer);  
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, TrainerToBeUpdatedModelView model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check data and missing fields");
                return View(model);
            }
            var flag = await _trainerService.UpdateTrainer(id,model);
            if (flag)
            {
                TempData["Message"] = "Trainer Updated Successfully";
            }else
            {
                TempData["Error"] = "Trainer Update failed";
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrainer(TrainerToBeCreatedModelView model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check data and missing fields");
                return View(model);
            }
            var flag = await _trainerService.CreateTrainer(model);
            if (flag)
            {
                TempData["Message"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["Error"] = "Trainer Creation failed";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flag = await _trainerService.DeleteTrainer(id);
            if (flag)
            {
                TempData["Message"] = "Trainer Deleted Successfully";
            }
            else
            { 
                TempData["Error"] = "Trainer Deletion failed";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
