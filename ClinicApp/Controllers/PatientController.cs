using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace ClinicApp.Controllers
{

    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientService patientService;
        
        public PatientController(IPatientService patientService) {
            this.patientService = patientService;
        }

        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        public async Task<IActionResult> Index()
        {
            var patients = (await patientService.GetAllAsync()).ToList();
            return View("Index", patients);
        }

        [HttpGet]
        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        public async Task<IActionResult> Create() {

            return View("Create");
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientCreateViewModel patientModel)
        {
            if (ModelState.IsValid)
            {
                var NewAddedPatient = await patientService.AddPatientasync(patientModel);
                TempData["NewUsername"] = NewAddedPatient.userName;
                TempData["NewPassword"] = patientModel.Password;
                return View(new PatientCreateViewModel());
            }

            return View("Create", patientModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        public async Task<IActionResult> Delete(int id, string patientSearch, string statusFilter)
        {
            await patientService.DeletePatietnAsync(id);

            // Persist values across redirect
            TempData["patientSearch"] = patientSearch;
            TempData["statusFilter"] = statusFilter;

            return RedirectToAction("Index");
        }

        [Authorize(Roles = ApplicationRole.AdminReceptionistPatient)]
        public async Task<IActionResult> Details(int id) {
            
            var patient = await patientService.GetPatientSessionsByIdAsync(id);
            if (patient is null) return NotFound();

            return View("Details", patient);

        }

        [HttpGet]
        [Authorize(Roles = ApplicationRole.AdminReceptionistPatient)]
        public async Task<IActionResult> Edit(int id ) {
            var viewModel = await patientService.GetPatientEditAsync(id);
            if (viewModel == null) return NotFound();
            return View("Edit" , viewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRole.AdminReceptionistPatient)]
        public async Task<IActionResult> Edit(PatientEditViewModel PatientModel) {
            if (!ModelState.IsValid) {
                if (!(ModelState.ErrorCount == 2 && !PatientModel.changePassword))
                    return View(PatientModel);
            
            }
            bool success = await patientService.SavePatientEditAsync(PatientModel);
            if (success)
            {
                TempData["SuccessMessage"] = "Patient updated successfully!"; // you can add json serializer in the details menu to show this message
                return RedirectToAction("Details" , new { id = PatientModel.ID});
            }
            ModelState.AddModelError("","Old Password isn't Correct..");
            return View(PatientModel);
        }
    
    }
}
