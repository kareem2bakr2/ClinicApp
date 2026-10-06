namespace ClinicApp.ViewModel
{
    public class RegisterViewModel : IValidatableObject
    {
        public List<SelectOption> AllTherapists { get; set; }
    = new List<SelectOption>();

        [Required(ErrorMessage ="Account type is required")]
        public AccountType? type { get; set; }

        [Required]
        [RegularExpression(@"^[^ ]+( +[^ ]+)+$", ErrorMessage = "Please Enter Your FullName.")]
        [Length(3, 100, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Length(10, 13 , ErrorMessage = "enter valid phone number")]
        public string Phone { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        public Gender Gender { get; set; }

        #region Reciptionest
        [Range(2, 90, ErrorMessage = "Age Must Be Between 2 and 90")]
        public int? Age { get; set; }
        public DateTime? WorktimeFrom { get; set; }
        public DateTime? WorkTimeTO { get; set; }
        public DayOfWeek? Dayoff { get; set; }

        #endregion

        #region Therapist
        
        public TherapistSpecialization? Specialization { get; set; }

        [Display(Name = "Seniority Level")]
        public SeniorityLevel? SeniorityLevel { get; set; }

        [Display(Name = "Hour Rate")]
        public decimal? hourRate { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerID { get; set; }
        #endregion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (type == AccountType.Receptionist)
            {
                if (Age is null)
                    yield return new ValidationResult("Age is required.", new[] { nameof(Age) });
                if (WorktimeFrom is null)
                    yield return new ValidationResult("Work time from is required.", new[] { nameof(WorktimeFrom) });
                if (WorkTimeTO is null)
                    yield return new ValidationResult("Work time to is required.", new[] { nameof(WorkTimeTO) });
                if (Dayoff is null)
                    yield return new ValidationResult("Day off is required.", new[] { nameof(Dayoff) });
            }
            else if (type == AccountType.Therapist)
            {
                if (Specialization is null)
                    yield return new ValidationResult("Specialization is required.", new[] { nameof(Specialization) });
                if (SeniorityLevel is null)
                    yield return new ValidationResult("Seniority level is required.", new[] { nameof(SeniorityLevel) });
                if (hourRate is null)
                    yield return new ValidationResult("Hour rate is required.", new[] { nameof(hourRate) });
            }
        }
    }

    
    public class SelectOption
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }

}
