using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    [Authorize(Roles = ApplicationRole.AdminTherapist)]
    public class ExerciseController : Controller
    {
        private readonly IExerciseService _ExerciseService;
        public ExerciseController(IExerciseService exerciseService)
        {
            _ExerciseService = exerciseService;
            
        }

        [HttpGet]
        public async Task< IActionResult> Index(ExcerciseIndexViewModel filter)
        {
            var model = await _ExerciseService.GetAllExercisesAsync(filter);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _ExerciseService.GetExerciseDetailsAsync(id);
            if (model == null) {
                TempData["ErrorMessage"] = "Can't Find Exercise.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ExerciseService.DeleteExerciseAsync(id);
            if (result.flag) TempData["SuccessMessage"] = result.message;
            else TempData["ErrorMessage"] = result.message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            var model = await _ExerciseService.GetAllCreateStaticDataAsync();
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseCreateModelView model) 
        {
            if(!ModelState.IsValid) return View(model);

            var result = await _ExerciseService.CreateExerciseAsync(model);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Error While Creating.";
                return View(model);
            }
            return RedirectToAction(nameof(Details), new { id = result.ID });

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var model = await _ExerciseService.GetExerciseDetailsAsync(id);    
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(ExerciseCreateModelView Model) {

            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            var result = await _ExerciseService.UpdateExerciseAsync(Model);        
            if(result is null) { 
                TempData["ErrorMessage"] = "Error ,Please Try agin Later."; 
                return View(Model);
            }
            TempData["SuccessMessage"] = "Saved Successfully";
            return RedirectToAction(nameof(Details), new {id = result.ID});
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddMuscle(int Exid , List<int> musclesIDs) {
            var result = await _ExerciseService.AddExerciseMuscleAsync(Exid , musclesIDs);
            
            if(result is null)  {
                TempData["ErrorMessage"] = "Please try again later.";
            }else
            TempData["SuccessMessage"] = "Saved Successfully";
            
            return RedirectToAction(nameof(Details), new { id = Exid });

        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddEquipments(int Exid, List<int> EquipmentsIDs)
        {
            var result = await _ExerciseService.AddExerciseEquipmentAsync(Exid, EquipmentsIDs);

            if (result is null)
            {
                TempData["ErrorMessage"] = "Please try again later.";
            }
            else
                TempData["SuccessMessage"] = "Saved Successfully";

            return RedirectToAction(nameof(Details), new { id = Exid });

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RemoveEquipment(int EXID ,int EquipID)
        {

            var result = await _ExerciseService.RemoveExerciseEquipmentAsync(EXID, EquipID);

            if (result is null)
            {
                TempData["ErrorMessage"] = "Please try again later.";
            }
            else
                TempData["SuccessMessage"] = "Saved Successfully";

            return RedirectToAction(nameof(Details), new { id = EXID });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RemoveMuscle(int EXID, int MuscleID)
        {

            var result = await _ExerciseService.RemoveExerciseMuscleAsync(EXID, MuscleID);

            if (result is null)
            {
                TempData["ErrorMessage"] = "Please try again later.";
            }
            else
                TempData["SuccessMessage"] = "Saved Successfully";

            return RedirectToAction(nameof(Details), new { id = EXID });
        }



    }
}
