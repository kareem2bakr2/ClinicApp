using ClinicApp.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace ClinicApp.ViewModel
{
    public class TherapistEditViewModel
    {
        public int DBId { get; set; }
        public string AppId { get; set; }

        
        public string PhotoURL { get; set; }

        [FileSize(1, [".png",".jpg",".jpeg"])]
        public IFormFile? Photo { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Length(10, 13 ,ErrorMessage ="Enter Valid Phone Number")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100 , ErrorMessage ="Enter Valid Email")]
        public string Email { get; set; }
        
        [Required]
        public TherapistSpecialization Specialization { get; set; }

        [Display(Name = "Seniority Level")]
        public SeniorityLevel SeniorityLevel { get; set; }

        public decimal _hourRate { get; set; }
        
        [Required]
        [Display(Name = "Hour Rate")]
        [GreaterNumber(nameof(_hourRate))]
        public decimal hourRate { get; set; }

        public bool changePasword { get; set; }
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-F]).{8,}$",
              ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character.")]
        public string NewPassword { get; set; }
        
    }
}
