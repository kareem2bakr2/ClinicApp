using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicApp.Controllers
{
    
    [Authorize(Roles = ApplicationRole.AdminTherapist)]
    public class MuscleController : Controller
    {
        private readonly IMuscleService muscleService;
        public MuscleController(IMuscleService muscleService) 
        {
            this.muscleService = muscleService;
        }

        public async Task<IActionResult> Index(MuscleIndexViewModel filter) 
        {
            var model =await muscleService.GetAllAsync(filter);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsJson(int id)
        {

            var model = await muscleService.GetDetails(id);
            return Json(model);


        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(MuscleViewModel model) {
            var result = await muscleService.EditMuscle(model);
            if (result.flag) {
                TempData["SuccessMessage"] = result.message;
            }
            else {
                TempData["ErrorMessage"] = result.message;
            }
            
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Add(MuscleViewModel model)
        {
            var result = await muscleService.AddMuscle(model);
            if (result.flag)
            {
                TempData["SuccessMessage"] = result.message;
            }
            else
            {
                TempData["ErrorMessage"] = result.message;
            }

            return RedirectToAction("Index");

        }

    }
}
