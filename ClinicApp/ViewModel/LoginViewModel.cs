namespace ClinicApp.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email or username is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="password is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        
    }
}
