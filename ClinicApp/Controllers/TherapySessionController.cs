using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    [Authorize]
    public class TherapySessionController : Controller
    {
        private readonly ITherapySessionService _sessionservice;
        public TherapySessionController(ITherapySessionService _sessionservice)
        {
            this._sessionservice = _sessionservice;
        }


        [Authorize(Roles = ApplicationRole.AllClinicRoles)]
        public async Task<IActionResult> Index(TherapySessionIndexModelView filter)
        {
            var model = await _sessionservice.GetAllIndexAsync(filter);
            return View(model);
        }

        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Cancel(int id) {
            var result = await _sessionservice.CancelSessionAsync(id);
            if (!result.flag)
            {
                TempData["ErrorMessage"] = result.message;
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = result.message.ToString();
            return RedirectToAction(nameof(Index));

        }


        [Authorize(Roles = ApplicationRole.AllClinicRoles)]
        public async Task<IActionResult> Details(int id)
        {
            var model =await  _sessionservice.GetDetialsAsync(id);
            
            if (model is null) return RedirectToAction(nameof(Index));
            ViewBag.AllTherapists = model.alltherapists;
            return View(model);
        }
        
        [Authorize(Roles = ApplicationRole.ClinicalStaff)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Complete(SessionCompleteViewModel model) 
        {
            var result = await _sessionservice.CompleteSessionAsync(model);
            if (!result.flag)
            {
                TempData["ErrorMessage"] = result.message;
            }
            else
            {
                TempData["SuccessMessage"] = result.message;
            }
            return RedirectToAction(nameof(Details), new { id = model.ID });

        }

        [Authorize(Roles = ApplicationRole.ClinicalStaff)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> MarkNoShow(int id) {

            var result = await _sessionservice.MarkAsNoShowAsync(id);
            
            if (!result.flag)
            {
                TempData["ErrorMessage"] = result.message;
            }
            else
            {
                TempData["SuccessMessage"] = result.message;
            }
            return RedirectToAction(nameof(Details), new { id });


        }

        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Reschedule (SessionResheduleViewMdel model)
        {
            var result  = await _sessionservice.ReScheduleSessionAsync(model);

            if (!result.flag)
            {
                TempData["ErrorMessage"] = result.message;
            }
            else
            {
                TempData["SuccessMessage"] = result.message;
            }
            return RedirectToAction(nameof(Details), new { id = model.ID });


        }

    }
}
