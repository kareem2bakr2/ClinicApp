using ClinicApp.Validation;
using System.ComponentModel;

namespace ClinicApp.ViewModel
{
    public class TherapistCreateViewModel
    {

        [Required]
        [DisplayName("Fisrt Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }


        [Required]
        [DisplayName("User Name")]
        public string userName { get; set; }


        [FileSize(5, [".png" , ".jpg" ,".jpeg"])]
        public IFormFile? Photo { get; set; }
        
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Length(10, 13 , ErrorMessage ="Enter Valid Phone Number")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        public Gender Gender { get; set; }
        [Required]
        
        public TherapistSpecialization Specialization { get; set; }

        [Required]
        [Display(Name = "Seniority Level")]
        public SeniorityLevel SeniorityLevel { get; set; }

        [Required]
        [Display(Name = "Hour Rate")]
        public decimal hourRate { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-F]).{8,}$",
                ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirming your password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }



    }
}
