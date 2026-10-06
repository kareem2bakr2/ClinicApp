using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{

    [Authorize(Roles = ApplicationRole.AdminReceptionist)]
    public class MedicalServiceController : Controller
    {
        private readonly IMedicalServiceService _medicalService;

        public MedicalServiceController(
            IMedicalServiceService medicalService 
            ) {
            this._medicalService = medicalService;
        }

        public async  Task<IActionResult> Index(MediaclServiceIndexViewModel searchModel)
        {
            var model = await _medicalService.GetAllMedicalService(searchModel);
            return View(model);
        }
        public async Task<IActionResult> Create() {

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(MedicalServiceViewModel model) 
        {
            if(!ModelState.IsValid) return View(model);
            
            var result = await _medicalService.CreateMedicalServiceAsync(model);
            if (result is null || result.ID == 0)
            {
                TempData["ErrorMessage"] = "Couldn't save new Medical Service";
                return View(model);
            }
            return RedirectToAction(nameof(Details) , new { id = result.ID});
        }

        public async Task<IActionResult> Details(int id , DateOnly StartDatefilter , DateOnly EndDatefilter)
        {
            var model = await _medicalService.GetMedicalServiceDetailsasync(
                new MedicalServiceDetailsViewModel {
                    id = id ,
                    StartDatefilter = StartDatefilter,
                    EndDatefilter = EndDatefilter});
            if (model is null) {
                TempData["ErrorMessage"] = "Can't Find Medical Service";
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id) {
            var model = await _medicalService
                .GetMedicalServiceDetailsasync(new MedicalServiceDetailsViewModel{id = id});
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(MedicalServiceViewModel model) {
            
            if (!ModelState.IsValid) return View(model);
            
            var result = await _medicalService.EditMedicalServiceAsync(model);
            
            if (!result.flag) { 
                TempData["ErrorMessage"] = "Couldn't Edit Medical Service";
                return View(result);
            }
         
            return RedirectToAction(nameof(Details) , new { id = model.id});
            
        }
    }
}
