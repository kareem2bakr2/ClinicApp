namespace ClinicApp.ViewModel
{
    public class PatientEditViewModel
    {
        public int ID { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string EmergencyPhone { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Enter Valid Name")]
        public string EmergencyContact { get; set; }

        [Required]
        public string EmergencyRelationShip { get; set; }
        
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public bool changePassword { get; set; }

        [DataType(DataType.Password)]
        public string oldPassword { get; set; } 

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-F]).{8,}$",
                ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirming your password is required.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }

    }
}
