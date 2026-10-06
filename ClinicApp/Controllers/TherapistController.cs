using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.NativeInterop;
using System.Security.Authentication;

namespace ClinicApp.Controllers
{
    [Authorize]
    public class TherapistController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITherapistService therapistService;
        private readonly ICurrentUserService _currentUserService;
        
        public TherapistController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITherapistService therapistService,
            ICurrentUserService currentUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.therapistService = therapistService;
            _currentUserService = currentUser;
        }


        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Index()
        {
            var user =  await _userManager.FindByIdAsync(_currentUserService.CurrentUserID);
            if (user == null) return NotFound();

            List<TherapistListViewModel> therapistListViewModel 
                = await  therapistService.GetMyStaffTherapistsAsync(user.Therapist?.ID) ?? new List<TherapistListViewModel>();
            
            return View("Index" , therapistListViewModel);
        }

         

        [HttpGet]
        [Authorize(Roles = ApplicationRole.Therapist)]
        public async Task<IActionResult> ADDToTeam() {
            var managerUser = await _userManager.FindByEmailAsync("manager@clinic.com");

            if (managerUser != null)
            {
                string managerId = managerUser.Id; // e.g., "a8b34c12-..." or 5

                await _signInManager.SignInAsync(managerUser, isPersistent: false);

                var therarpists = await therapistService.GetTherapistWithNoMGR(managerUser.Therapist.ID);

                return View("ADDToTeam", therarpists);

            }

            return NoContent();

            //var therarpists = await therapistService.GetTherapistWithNoMGR();
            //return View("ADDToTeam", therarpists);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.Therapist)]
        public async Task<IActionResult> ADDToTeam(int[] ids) {

            var managerid = _currentUserService.CurrentUserID;
            if (managerid is null) NotFound("Manager Not Found");
            try
            {
                await therapistService.ChangeTherapistTeam(ids, managerid);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                ViewBag.ids = ids;
                return RedirectToAction("ADDToTeam");
            }

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = ApplicationRole.Therapist)]
        public async Task<IActionResult> RemoveFromTeam(int id, int managerid)
        {
            await therapistService.removeFromTeamAsync(id, managerid);
            return RedirectToAction("Index");
            
        }

        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Details(int id) 
        {
            var therapistInfo  = await therapistService.GetTherapistDetialsAsync(id);               
            return View("Details" , therapistInfo);
            
 
        }

        [HttpGet]
        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Edit(int id) {
            var therapist = await therapistService.GetTherapistEditAsync(id); 
            return View("Edit", therapist);
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRole.AdminTherapist)]
        public async Task<IActionResult> Edit(TherapistEditViewModel therapistModel , int[] SelectedSpecializations) 
        {
            TherapistSpecialization combinedFlags = 0;
            if (SelectedSpecializations != null)
            {
                foreach (var val in SelectedSpecializations)
                {
                    combinedFlags |= (TherapistSpecialization)val;
                }
            }
            therapistModel.Specialization = combinedFlags;

            if ((therapistModel.changePasword && !ModelState.IsValid) 
                ||(!therapistModel.changePasword && ModelState.ErrorCount > 1) 
                ) { 
                return View(therapistModel);
            }


            var ErrorState = await therapistService.SaveEditAsync(therapistModel);

            if (ErrorState) {
                ModelState.AddModelError("","Error in Saving Edits");
                return View(therapistService);

            }
            return RedirectToAction(nameof(Details),new { id = therapistModel.DBId });
        

        }

        [HttpGet]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Create() { 
            
            return View();
            
        }
        [HttpPost]
        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Create(TherapistCreateViewModel therpaistModel , int[] SelectedSpecializations) 
        {
            if (!ModelState.IsValid) return View(therpaistModel);
            //ITherapistService
            //TherapistService
            TherapistSpecialization therapistSpecialization = 0;
            if (SelectedSpecializations is not null) {
                foreach (var val in SelectedSpecializations) {
                    therapistSpecialization |= (TherapistSpecialization)val;
                }
            }
            //TherapistService
            therpaistModel.Specialization = therapistSpecialization;
            try 
            {
                var therapist = await therapistService.AddNewTherapistAsync(therpaistModel);
                return RedirectToAction(nameof(Details), new { id = therapist.ID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(therpaistModel);

            }

        }

    }


}
