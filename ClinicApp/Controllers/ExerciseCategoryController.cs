using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{

    [Authorize(Roles = ApplicationRole.AdminTherapist)]
    public class ExerciseCategoryController : Controller
    {
        private readonly IExerciseCategoryService ExerciseService;
        public ExerciseCategoryController( IExerciseCategoryService ExerciseService) {
        
            this.ExerciseService = ExerciseService;
            
        }

        public async  Task<IActionResult> Index(ExerciseCategoryIndexModelView filter)
        {
            var model = await ExerciseService.GetAllCategoriresAsync(filter);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsJson(int id)
        {
            var model = await ExerciseService.GetExerciseCategoryDetailsAsync(id);
            if (model == null) return NotFound();

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await ExerciseService.DeleteExerciseAsync(id);
            if (result.flag)
            {
                TempData["SuccessMessage"] = result.message;
            }
            else
            {
                TempData["ErrorMessage"] = result.message;
            }
            return RedirectToAction( nameof( Index ) );
        }
        public async Task<IActionResult> Edit(ExerciseCategoryModelView model)
        {

            var result = await ExerciseService.EditExerciseCategoryAsync(model);
            if (result is null)
                TempData["ErrorMessage"] = "Error ,Please Try again Later";
            else TempData["SuccessMessage"] = "Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create(ExerciseCategoryModelView model)
        {
        
            if(!ModelState.IsValid)
                TempData["ErrorMessage"] = $"Failed To Create : {ModelState.Values.SelectMany(e=>e.Errors).Select(e=>e.ErrorMessage).FirstOrDefault()}";
            else TempData["SuccessMessage"] = "Creted Successfully";
                return RedirectToAction(nameof(Index));
        
        }
        //125+80+140

    }
}
