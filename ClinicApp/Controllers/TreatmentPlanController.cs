using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    [Authorize]
    public class TreatmentPlanController : Controller
    {

        private readonly ITreatmentPlanService _treatmentPlanService;
        public TreatmentPlanController(
            ITreatmentPlanService _treatmentPlanService) { 
            this._treatmentPlanService  = _treatmentPlanService;
        }

        [Authorize(Roles = ApplicationRole.AdminTherapistPatient)]
        public async Task<IActionResult> Index(TreatmentPlanIndexModelView fitler)
        {
            var model = await _treatmentPlanService.GetAllTreatmentPlansAsync(fitler);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Create(TreatmentPlanCreateModelview model) 
        {

            if (!ModelState.IsValid) 
            { 
                return View(model);
            }
            
            var tp = await _treatmentPlanService.CreateTreatmentPlanAsync(model); 
           
            if(tp is null) { 
                TempData["ErrorMessage"] = "Error While Creating.";    
                return View(model); 
            }
           
            TempData["SuccessMessage"] = "Saved Successfully";

            return RedirectToAction(nameof(Details), new { id = tp.ID });
        }

        [Authorize(Roles = ApplicationRole.AdminTherapistPatient)]
        public async Task<IActionResult> Details(int id) 
        {
            var tp = await _treatmentPlanService.GetDetailsAsync(id);
            if(tp is null)
            {
                TempData["ErrorMessage"] = "Can't Find Treatment Plan";
                return RedirectToAction(nameof(Index));
            }
            return View(tp);
        }

        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Delete(int id) {
            var result  = await _treatmentPlanService.DeleteTreatmentPlanAsync(id);
            if (!result.flag)
            {
                TempData["ErrorMessage"] = result.message;
            }
            else
            {
                TempData["SuccessMessage"] = result.message;
            }
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> AddExercises(int treatmentPlanId, List<int> exerciseIds) 
        {
            var result = await _treatmentPlanService.AddNewExerciseAsync(treatmentPlanId, exerciseIds);
            
            if(result is null) TempData["ErrorMessage"] = "Failed To Add Data";
            else TempData["SuccessMessage"] = "Saved Successfully";
            
            return RedirectToAction(nameof(Details), new { id = treatmentPlanId });
        }

        [HttpGet]
        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _treatmentPlanService.GetDetailsAsync(id);
            return View(entity);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Edit(TreatmentPlanCreateModelview model) {
            if(!ModelState.IsValid)
                return View(model);
            var result = await _treatmentPlanService.EditTreatmentPlanAsync(model);
            if (result is null) TempData["ErrorMessage"] = "Failed To Add Data";
            else TempData["SuccessMessage"] = "Saved Successfully";

            return RedirectToAction(nameof(Details), new { id = model.ID });

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> RemoveExercise(int exerciseId , int treatmentPlanId) 
        {
            var result = await _treatmentPlanService.RemoveExerciseAsync(treatmentPlanId,exerciseId);
            
            if (!result.flag) TempData["ErrorMessage"] = "Failed To Add Data";
            else TempData["SuccessMessage"] = "Saved Successfully";

            return RedirectToAction(nameof(Details), new { id = treatmentPlanId });
        }
    }
}
