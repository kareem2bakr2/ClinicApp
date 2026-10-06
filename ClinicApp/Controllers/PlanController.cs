using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    [Authorize(Roles = ApplicationRole.Admin)]
    public class PlanController : Controller
    {

        private readonly IPlanService _planService;
        public PlanController(
            IPlanService planService
            ) { 
            _planService = planService;
        }


        public async Task<IActionResult> Index(PlanIndexModelView filter)
        {
            var model = await _planService.GetAllPlansAsync(filter);
            return View(model);
        }
        
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(PlanListModelView model)
        {
            
            if (!ModelState.IsValid) 
                return View(model);
            var plan = await _planService.AddPlanAsync(model);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Error Couldn't complete actiono";
                return View(model);
            }
            TempData["SuccessMessage"] = "Created successfully";
            return RedirectToAction(nameof(Details) , new {id = plan.ID});
            
        }
        public async Task<IActionResult> Details(int id)
        {
            var plan = await _planService.GetPlanDetailsAsync(id);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Couldn't Find Plan ";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _planService.DeletePlanAsync(id);
            if (!result.flag)
            {
                TempData["ErrorMessage"] = result.message;
                return RedirectToAction(nameof(Details), new {id});
            }

            TempData["SuccessMessage"] = result.message;

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _planService.GetPlanDetailsAsync(id);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(PlanDetailsModelView model)
        {
            if(!ModelState.IsValid)
                return View(model);
            var result = await _planService.EditPlanAsync(model);
            if (!result.flag)
                TempData["ErrorMessage"] = result.message;
            else
                TempData["SuccessMessage"] = result.message;
            return RedirectToAction(nameof(Details), new {id = model.ID} );

        }
    
    
    }
}
