using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class MedicalService:ILogsAttribuite
    {

        #region log flags
        public bool isDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? Lastmodified { get; set; }

        [ForeignKey("CreatedBY")]
        public string? CreatedById { get; set; }

        [ForeignKey("Modifiedby")]
        public string? modefiedById { get; set; }

        public virtual ApplicationUser Modifiedby { get; set; }

        public virtual ApplicationUser CreatedBY { get; set; }

        #endregion

        [Key]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[^ ]+( +[^ ]+)+$", ErrorMessage = "Please Enter Your FullName.")]
        [Length(3, 100, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
        
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Discount must be a positive value.")]
        public decimal Discount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name= "Start Date Contract")]
        public DateOnly StartDateContract { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name= "End Date Contract")]
        public DateOnly? EndDateContract { get; set; }

        public virtual HashSet<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public virtual HashSet<Payment> Payments { get; set; }= new HashSet<Payment>();
    }
}
