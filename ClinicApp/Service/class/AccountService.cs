using ClinicApp.Comman;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using static ClinicApp.Comman.CommanExtensions;


namespace ClinicApp.Service
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ITherapistRepository _therapistRepo;
        private readonly IReceptionistRepository _receptionistRepo;
        private readonly IEmailService _emailService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AccountService(UserManager<ApplicationUser> UserManager,
                ITherapistRepository therapistRepository,
                IReceptionistRepository receptionistRepo,
                IEmailService emailService,
                IHttpContextAccessor httpContextAccessor,
                ICurrentUserService currentUserService,
                SignInManager<ApplicationUser> signInManager
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
            _signInManager = signInManager;
            _emailService = emailService;
            _receptionistRepo = receptionistRepo;
            _UserManager = UserManager;
            _therapistRepo = therapistRepository;
        }

      
        public async Task<RegisterViewModel> GetRegisterStaticDataAsync()
        {
            return new RegisterViewModel
            {
                AllTherapists =
                    _therapistRepo
                    .GetAll()
                    .Select(t => new SelectOption { ID = t.ID, Name = t.Name })
                    .ToList()
            };
        }

        public async Task<ReturnResult> RegisterAsync(RegisterViewModel model)
        {
            if (model.type is null) return new ReturnResult { flag = false, message = "Account type is required." };

            var SameMailuser = await _UserManager.FindByEmailAsync(model.Email);
            if (SameMailuser != null) return new ReturnResult { flag = false, message = "There is an account Linked With this Email." };

            var user = new ApplicationUser { 
                Email = model.Email  , 
                UserName = model.Email,
                PhoneNumber = model.Phone,
                type = model.type.Value
            };
            
            var password = GeneratePassword();
            var result  = await _UserManager.CreateAsync(user,password);
            if (!result.Succeeded) {
                return new ReturnResult { flag = false, message = result.Errors.FirstOrDefault().Description};
            }

            if (model.type == AccountType.Receptionist)
            {
                await _UserManager.AddToRoleAsync(user, ApplicationRole.Receptionist);
                var recept = new Receptionist { 
                    Age = model.Age ?? 0  ,
                    AppUserId = user.Id,
                    appUser = user , 
                    Dayoff = model.Dayoff ?? DayOfWeek.Friday ,
                    Gender = model.Gender,
                    Name = model.Name ,
                    Phone = model.Phone ,
                    RequirePasswordChange = true,
                    WorktimeFrom = model.WorktimeFrom ?? DateTime.Today ,
                    WorkTimeTO = model.WorkTimeTO ?? DateTime.Today,
                    
                };
                
                await _receptionistRepo.AddAsync(recept);
            }   
            else if (model.type == AccountType.Therapist) 
            {
                await _UserManager.AddToRoleAsync(user, ApplicationRole.Therapist);

                var therapist = new Therapist {
                    
                    Email = model.Email ,
                    Gender = model.Gender ,
                    hourRate = model.hourRate ?? 0,
                    ManagerID = model.ManagerID ,
                    Phone = model.Phone ,
                    AppUserId = user.Id,
                    Name = model.Name ,
                    RequirePasswordChange=true,
                    SeniorityLevel = model.SeniorityLevel ?? default ,
                    Specialization = model.Specialization ?? default ,
                };
                await _therapistRepo.AddAsync(therapist);
            }

            var token =await _UserManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedTocken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));

            await _receptionistRepo.SaveAsync();
            var confirmLink = $"{GetAppBaseUrl()}/Account/confirmemail?userId={user.Id}&token={encodedTocken}";
            await _emailService.SendAsync(null, model.Email, 
                "Confirm Email", $"<p>Dear {model.Name},</p>" +
                $"<p>please use the following credintials to change password</p>" +
                $"<p>email : {model.Email}<br/>password : {password}</p>" +
                $"<p>Click here to confirm email : <a href=\"{confirmLink}\">click here</a></p>");
            return new ReturnResult{ flag = true ,message = "User Created Successfully"};
            //email service to sened email and password 
        }
    
        public async Task<ReturnResult> ConfirmEmailAsync(ConfirmEmailViewModel model)
        {
            var user =await  _UserManager.FindByIdAsync(model.userId);
            if (user is null) return new ReturnResult { flag = false, message = "Failed to Confirm Email" };
            if (model.token is null) return new ReturnResult { flag = false, message = "Failed to Confirm Email" };


            var decodedToken = Encoding.UTF8.GetString( 
                WebEncoders
                .Base64UrlDecode(
                    model.token)
            );

            if (!user.EmailConfirmed)
            {
                var ConfirmEmailresult = await _UserManager.ConfirmEmailAsync(user, decodedToken);
                if (!ConfirmEmailresult.Succeeded) return new ReturnResult { flag = false, message = ConfirmEmailresult.Errors.FirstOrDefault()?.Description ?? "Failed to Confirm Email" };
            }

            if (user.type == AccountType.Receptionist)
            {
                var recept = user.Receptionist;
                if (recept is null) return new ReturnResult { flag = false, message = "Please Try again." };
                recept.RequirePasswordChange = false;
            }
            else if (user.type == AccountType.Therapist) {
                var therapist = user.Therapist;
                if (therapist is null) return new ReturnResult { flag = false, message = "Please Try again." };
                therapist.RequirePasswordChange = false;
            }
            await _receptionistRepo.SaveAsync();

            var ChangePasswordResult = await _UserManager.ChangePasswordAsync(user,model.OldPassword , model.NewPassword);
            if (!ChangePasswordResult.Succeeded) return new ReturnResult { flag = false, message = ChangePasswordResult.Errors.FirstOrDefault().Description };

            return new ReturnResult { flag = true, message = "Email Confirmed Successfully" };
        }

        public async Task<ReturnResult> Login(LoginViewModel model) {

            ApplicationUser user;
            if(new EmailAddressAttribute().IsValid(model.Email))
            {
                user = await _UserManager.FindByEmailAsync(model.Email);
            }
            else
            {
                user = await _UserManager.FindByNameAsync(model.Email);
            }
            
            if (user is null) return new ReturnResult { flag = false, message = "Incorrect email or password" };

            if(user.type == AccountType.Receptionist && user.Receptionist is not null && user.Receptionist.RequirePasswordChange)
                return new ReturnResult { flag = false, message = "Please Confirm email." };

            if (user.type == AccountType.Therapist && user.Therapist is not null && user.Therapist.RequirePasswordChange)
                return new ReturnResult { flag = false, message = "Please Confirm email." };
                
            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
                return new ReturnResult { flag = false, message = $"Account Locked Until { user.LockoutEnd }." };


            if (!await _UserManager.CheckPasswordAsync(user, model.Password))
            {
                await _UserManager.AccessFailedAsync(user);
                return new ReturnResult { flag = false, message = "Incorrect email or password" };
            }

            await _UserManager.ResetAccessFailedCountAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

            return new ReturnResult { flag = true, message = "logged successfully" };
        }

        public async Task<ReturnResult> Logout() {
            var user = await _UserManager.FindByIdAsync(_currentUserService.CurrentUserID);
            if (user is null) return new ReturnResult { flag = false, message = "You aren't signed in." };
            
            await _signInManager.SignOutAsync();
            
            return new ReturnResult { flag = true, message = "Loged out" };

        }
        public async Task<ReturnResult> SendResetPassword(string email) {
            
            var user  =await  _UserManager.FindByEmailAsync(email);
            
            if (user is null) return new ReturnResult { flag = false, message = "can't Find User" };

            var token = await _UserManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode( 
                    Encoding.UTF8.GetBytes(token)
                );

            var resetLink = $"{GetAppBaseUrl()}/Account/resetpassword?userId={user.Id}&token={encodedToken}";
            var emailBody = $@"
                        <div style='max-width:600px; margin:40px auto; background:#ffffff; 
                                    padding:40px; border-radius:10px;'>

                            <h2 style='color:#333333; margin-top:0;'>
                                Reset Your Password
                            </h2>

                            <p style='color:#555555; font-size:16px; line-height:1.6;'>
                                Hello {user.UserName},
                            </p>

                            <p style='color:#555555; font-size:16px; line-height:1.6;'>
                                We received a request to reset the password for your Medico Clinic account.
                            </p>

                            <p style='color:#555555; font-size:16px; line-height:1.6;'>
                                Click the button below to create a new password:
                            </p>

                            <div style='text-align:center; margin:30px 0;'>
                                <a href='{resetLink}'
                                   style='background:#007bff; color:#ffffff;
                                          padding:12px 25px; text-decoration:none;
                                          border-radius:6px; display:inline-block;
                                          font-size:16px;'>
                                    Reset Password
                                </a>
                            </div>

                            <p style='color:#777777; font-size:14px; line-height:1.5;'>
                                This link is for resetting your password. If you did not request
                                a password reset, you can safely ignore this email.
                            </p>

                            <hr style='border:none; border-top:1px solid #eeeeee; margin:30px 0;'>

                            <p style='color:#999999; font-size:12px; text-align:center;'>
                                © 2026 LMS. All rights reserved.
                            </p>

                        </div>
                        ";
            await _emailService.SendAsync(null, user.Email, "Reset Password", emailBody);

            return new ReturnResult { flag = true, message = "Email Sent" };

        }
        public async Task<ReturnResult> DeleteAsync(string userid)
        {
            var user = await _UserManager.FindByIdAsync(userid);
            if (user is null) return new ReturnResult { flag = false, message = "Failed , please try again later." };
            if(user.type == AccountType.Receptionist)
            {
                if (user.Receptionist is null) return new ReturnResult { flag = false, message = "Failed , please try again later." };
                user.Receptionist.isDeleted = true;
            }
            if(user.type == AccountType.Patient)
            {
                if (user.Patient is null) return new ReturnResult { flag = false, message = "Failed , please try again later." };
                user.Patient.isDeleted = true;
            }
            if(user.type == AccountType.Therapist)
            {
                if (user.Therapist is null) return new ReturnResult { flag = false, message = "Failed , please try again later." };
                user.Therapist.isDeleted = true;
            }

            await _therapistRepo.SaveAsync();
            return new ReturnResult { flag = true, message = "Deleted Successfully." };
        } 
        public async Task<ReturnResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _UserManager.FindByIdAsync(model.userId);

            if (user is null) return new ReturnResult { flag = false, message = "Erorr,Please Try again Later" };

            var decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(model.token)
                );
            
            if (decodedToken is null) return new ReturnResult { flag = false, message = "Erorr,Please Try again Later" };

            var res = await _UserManager.ResetPasswordAsync(user,decodedToken,model.NewPassword);
            if (!res.Succeeded) return new ReturnResult { flag = false, message = res.Errors.FirstOrDefault().Description };
        
            return new ReturnResult { flag = true, message = "Password Changed Successfully." };
        }

        public async Task<AccountIndexViewModel> GetAllUsersAsync(AccountIndexViewModel filter)
        {
            var items = _UserManager.Users.Where(
                        u => (u.type == AccountType.Receptionist && u.Receptionist != null && !u.Receptionist.isDeleted) ||
                        (u.type == AccountType.Therapist && u.Therapist != null && !u.Therapist.isDeleted) ||
                        (u.type == AccountType.Patient && u.Patient != null && !u.Patient.isDeleted)
                        );
            if (filter.TypeFilter is not null) items = items.Where(u => u.type == filter.TypeFilter);

            if (filter.SearchName is not null)
                items = items.Where(u =>
                    u.UserName.Contains(filter.SearchName) ||
                    u.Email.Contains(filter.SearchName) ||
                    (u.Therapist != null && u.Therapist.Name.Contains(filter.SearchName)) ||
                    (u.Receptionist != null && u.Receptionist.Name.Contains(filter.SearchName))
                    );

            if (filter.PageIndex < 1) filter.PageIndex = 1;
            if (filter.PageSize < 1) filter.PageSize = 20;

            filter.TotalCount =(int)Math.Ceiling( (decimal) items.Count()/filter.PageSize);
            filter.allUsers = items
                                .Skip((filter.PageIndex - 1) * filter.PageSize)
                                .Take(filter.PageSize)
                                .Select(u => new AccountDetialsViewModel {
                                    email = u.Email ,
                                    type = u.type , 
                                    UserId = u.Id,
                                    id = u.Receptionist != null ? u.Receptionist.ID
                                   : u.Therapist != null ? u.Therapist.ID
                                   : u.Patient != null ? u.Patient.ID
                                   : 0,username = u.UserName , 
                                     age = u.Receptionist == null ? 0: u.Receptionist.Age})
                                .ToList();

            return filter;
            
        }

        private string GetAppBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request is null) return FRONTENDURL;
            return $"{request.Scheme}://{request.Host}";
        }
   
    
    }
}
