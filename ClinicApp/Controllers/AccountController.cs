using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    //add user controller
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager) {
            _accountService = accountService;
            _userManager = userManager;
        }

        [Authorize(Roles =ApplicationRole.Admin)]
        [HttpGet]
        public async Task<IActionResult> Register() 
        {
            var model = await _accountService.GetRegisterStaticDataAsync();
            return View(model);
        }
        
        [Authorize(Roles = ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                var staticData = await _accountService.GetRegisterStaticDataAsync();
                model.AllTherapists = staticData.AllTherapists;
                return View(model);
            }

            var result = await _accountService.RegisterAsync(model);
            if (!result.flag) {
                TempData["ErrorMessage"] = result.message;
                var staticData = await _accountService.GetRegisterStaticDataAsync();
                model.AllTherapists = staticData.AllTherapists;
                return View(model);
            }

            TempData["SuccessMessage"] = result.message;
            return RedirectToAction(nameof(Index));

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> confirmemail(string userId, string token) 
        {
            return View(new ConfirmEmailViewModel { userId = userId , token = token});      
        }
        
        [AllowAnonymous]
        [ValidateAntiForgeryToken , HttpPost]
        public async Task<IActionResult> confirmemail(ConfirmEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var res = await _accountService.ConfirmEmailAsync(model);
            
            if (!res.flag) {
                TempData["ErrorMessage"] = res.message;
                return View(model);
            }
            
            TempData["SuccessMessage"] = res.message;
            return RedirectToAction(nameof(Login));
        }


        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Index(AccountIndexViewModel filter) 
        
        {
            var model = await _accountService.GetAllUsersAsync(filter);
            return View(model);
        }

        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> Delete(string id) 
        {
            var res = await _accountService.DeleteAsync(id);
            return Json(res);
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            return View();
        }


        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            if (!ModelState.IsValid)return View(model);
            var res = await _accountService.Login(model);

            if (!res.flag) {
                TempData["ErrorMessage"] = res.message;
                return View(model);
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        
        [ValidateAntiForgeryToken,HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction(nameof(Index), "Home");
        }


        [Authorize(Roles = ApplicationRole.Admin)]
        public async Task<IActionResult> SendResetToken(string email) {
            var res = await _accountService.SendResetPassword(email);
            if (!res.flag) {
                TempData["ErrorMessage"] =res.message;
                return Json(new {flag=false , result = res.message });
            }
            TempData["SuccessMessage"] = res.message;
            return Json(new { flag = true, result = res.message });
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> resetpassword(string userid ,string token)
        {
            return View (new ResetPasswordViewModel { userId = userid , token = token});
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> resetpassword(ResetPasswordViewModel model)
        {
            var res = await _accountService.ResetPassword(model);
            if (!res.flag)
            {
                TempData["ErrorMessage"] = res.message;
                return View(model);
            }
            TempData["SuccessMessage"] = res.message;
            return RedirectToAction(nameof(Login));
        }
        //users and roles controller 
        //adjust allowing paging according to roles 

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if(user.Patient != null)
            {
                return RedirectToAction("Details" ,"Patient", new { id = user.Patient.ID });
            }
            else if (user.Therapist != null)
            {
                return RedirectToAction("Details" , "Therapist", new { id = user.Therapist.ID });
            }
            else if (user.Receptionist != null)
            {
                return RedirectToAction("Details", "Receptionist", new { id = user.Receptionist.ID });
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
