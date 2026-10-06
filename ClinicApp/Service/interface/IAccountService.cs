using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Service
{
    public interface IAccountService
    {
        Task<RegisterViewModel> GetRegisterStaticDataAsync();
        Task<ReturnResult> RegisterAsync(RegisterViewModel model);
        Task<ReturnResult> ConfirmEmailAsync(ConfirmEmailViewModel model);
        Task<ReturnResult> Login(LoginViewModel model);
        Task<ReturnResult> Logout();
        Task<ReturnResult> SendResetPassword(string email);
        Task<ReturnResult> ResetPassword(ResetPasswordViewModel model);
        Task<AccountIndexViewModel> GetAllUsersAsync(AccountIndexViewModel filter);
        Task<ReturnResult> DeleteAsync(string userid);

    }
}
