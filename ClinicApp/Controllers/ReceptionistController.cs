using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    [Authorize]
    public class ReceptionistController : Controller
    {
        IReceptionistService _receptService;
        public ReceptionistController(IReceptionistService receptionistService) 
        {
            _receptService = receptionistService;
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Index()
        {
            var receptionist = await _receptService.GetReceptionistListViewModelAsync();
            return View(receptionist);
        }

        [HttpGet]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Create() {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Create(ReceptionistCreateViewModel ReceptModel) {
            try 
            {
                var recept =  await _receptService.CreateNewReceptionistAsync(ReceptModel);
                return RedirectToAction("Details" , new {id = recept.ID});
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(ReceptModel);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Delete(int id) { 
            
            var result = await _receptService.DeleteAsync(id);
            if (!result)
            {
                TempData["ErrorMessage"] = "Error While deleting Receptionist";
            }
            else {
                TempData["SuccessMessage"] = "Delete Completed Sunccessfully";
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        public async Task<IActionResult> Details(int id)
        {
            var receptionist = await _receptService.GetReceptionistDetailsAsync(id);
            return View(receptionist);
            
        }
        
        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var receptionist = await _receptService.GetReceptionistEditAsync(id);
            return View(receptionist);
        }


        [Authorize(Roles = ApplicationRole.AdminReceptionist)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(ReceptionistEditViewModel receptionist) {
            if (!receptionist.changePasword)
            {
                ModelState.Remove(nameof(receptionist.ConfirmNewPassword));
                ModelState.Remove(nameof(receptionist.NewPassword));

            }
            if (!ModelState.IsValid) { 
                return View(receptionist);
            }
            var result = await _receptService.SaveReceptionistEditAsync(receptionist);
            if (result) {
                TempData["SuccessMessage"] = "Receptionist Saved Successfully.";
                return RedirectToAction(nameof(Details),new {id = receptionist.ID});
            }
            ModelState.AddModelError("", "Error While Saving Edits.");
            
            return View(receptionist);

        } 


    }
}
