using System.ComponentModel;

namespace ClinicApp.ViewModel
{
    public class ReceptionistEditViewModel
    {

        public int ID { get; set; }

        public string PhotoURL { get; set; }
        [FileSize(5, [".png" , ".jpeg" , ".jpg"] )]
        public IFormFile Photo { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public DateTime WorktimeFrom { get; set; }
        public DateTime WorkTimeTO { get; set; }
        public DayOfWeek Dayoff { get; set; }

        public bool changePasword { get; set; }
        
        
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
