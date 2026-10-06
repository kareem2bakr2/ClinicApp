using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ClinicApp.Controllers
{

    [Authorize]
    public class AppointmentController : Controller
    {

        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService _appointmentService) { 
            this._appointmentService = _appointmentService;
        }

        [Authorize(Roles = ApplicationRole.AdminReceptionistPatient)]

        public async Task<IActionResult> Index(FilterAppointmentListViewModel filter)
        {
            var model = await _appointmentService.GetAppointmentsAsync(filter);
            ViewBag.Data = model;
            return View(filter);
        }


        [Authorize(Roles = ApplicationRole.AdminReceptionistPatient)]
        public async Task<IActionResult> Details(int id)
        {
            var model = await  _appointmentService.GetAppointmentDetailsAsync(id);
            if (model is null) return NotFound();
            return View(model);
        }

        //[ValidateAntiForgeryToken]
        
        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        [HttpPost]
        public async Task<IActionResult> CompleteAppointment(CompleteAppointmentViewModel viewModel) 
        {
            var result = await _appointmentService.ScheduleAppointmentAsync(viewModel);
            if (result.flag)TempData["SuccessMessage"] = result.message;
            else TempData["ErrorMessage"] = result.message;
            
            return RedirectToAction(nameof(Details) , new { id = viewModel.ID});
            
        }

        [Authorize(Roles = ApplicationRole.AdminReceptionist)]

        public async Task<IActionResult> Cancel(int id) 
        {
            var result = await _appointmentService.CancelAppointmenAsync(id);
            if (result.flag) TempData["SuccessMessage"] = result.message;
            else TempData["ErrorMessage"] = result.message;
            return RedirectToAction(nameof(Details), new { id = id });

        }

        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        public async Task<IActionResult> Pay(AppointmentPayViewModel model) 
        {
            var result = await _appointmentService.PayAppointmentByPatientAsync(model);

            if (result.flag) TempData["SuccessMessage"] = result.message;
            else TempData["ErrorMessage"] = result.message;
            return RedirectToAction(nameof(Details), new { id = model.ID });

        }

        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        public async Task<IActionResult> EditNotesAndDates(AppointmentEditViewModel model) { 
        
            var result = await _appointmentService.EditAppointmentAsync(model);
            if (result.flag) TempData["SuccessMessage"] = result.message;
            else TempData["ErrorMessage"] = result.message;
            return RedirectToAction(nameof(Details), new { id = model.ID });

        }

        [HttpGet]
        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        public async Task<IActionResult> Add(int patientId) 
        {
            var model = await _appointmentService.GetCreateStaticDataAsync(patientId);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        public async Task<IActionResult>Add(AppointmentCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var appointment = await _appointmentService.AddAppointmentAsync(model);
            if(appointment is null)
            {
                TempData["ErrorMessage"] = "Couldn't Create Appointment";
                return View(model);
            }
            return RedirectToAction(nameof(Details), new { id = appointment.ID });
        }

    }
}
