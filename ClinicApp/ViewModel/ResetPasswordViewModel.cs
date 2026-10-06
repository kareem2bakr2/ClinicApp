namespace ClinicApp.ViewModel
{
    public class ResetPasswordViewModel
    {
        public string userId { get; set;}
        public string token { get; set;}
        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, MinimumLength = 8,
        ErrorMessage = "New password must be between 8 and 100 characters.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your new password.")]
        [Compare(nameof(NewPassword),
            ErrorMessage = "New password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
