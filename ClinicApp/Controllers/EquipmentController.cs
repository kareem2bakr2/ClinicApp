
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{

    [Authorize(Roles = ApplicationRole.AdminReceptionist)]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _equipmentService;
        public EquipmentController(
            IEquipmentService _equipmentService ) 
        {
            this._equipmentService = _equipmentService;
        }

        public async Task<IActionResult> Index(ItemIndexViewModel model)
        {
            var equipments = await _equipmentService.GetAllEquipmentsAsync(model);
            return View(equipments);
        }

        public async Task<IActionResult> Create() {
            var exercises = await _equipmentService.CreateEquipmentStaticData();
            return View(exercises);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(EquipmentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _equipmentService.AddEquipmentAsync(model);
            if(result is null || result.ID == 0 ) return View(model);

            return RedirectToAction(nameof(Details), new { id = result.ID });
        }

        public async Task<IActionResult> Details(int id) 
        {
            var equipment = await _equipmentService.GetEquipmentDetailsByIdAsync(id);
            if (equipment is null)
            {
                TempData["ErrorMessage"] = "Equipment not found" ;
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var result = await _equipmentService.DeleteEquipmentAsync(id);
            if (!result.flag)
            {
                TempData["ErrorMessage"] = result.message;
                return RedirectToAction(nameof(Details), new { id = id });
            }

            TempData["SuccessMessage"] = result.message;
            return RedirectToAction(nameof(Index));
            
        }
        public async Task<IActionResult> Edit(int id) 
        {
            var details = await _equipmentService.GetEquipmentDetailsByIdAsync(id);
            ViewBag.allExercise =  (await _equipmentService.CreateEquipmentStaticData()).excersices;
            return View(details);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(EquipmentViewModel model) {
            var result = await _equipmentService.EditEquipmentAsync(model);
            if (!result.flag) {
                TempData["ErrorMessage"] = result.message;
                return View(model);
            }

            TempData["SuccessMessage"] = result.message;
            return RedirectToAction(nameof(Details) , new {id = model.Id});
        }
    }
}
