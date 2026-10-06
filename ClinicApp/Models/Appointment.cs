
using ClinicApp.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Models
{

    public class Appointment : ILogsAttribuite
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

        public AppointmentStatus Status { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name ="Start Date")]
        public DateTime? StartDate { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name ="End Date")]
        public DateTime? EndDate { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9\s.,]+$", ErrorMessage = "Please enter a valid note.")]
        [DataType(dataType:DataType.MultilineText)]
        [MaxLength(1000)]
        public string Notes { get; set; }
        
        public decimal? Discount { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        public decimal? DiscountPercentage { get; set; }
        
        [Display(Name ="Final Price")]
        public decimal? FinalPrice { get; set; }

        [ForeignKey("Receptionist")]
        public int? ReceptID { get; set; }
        
        [ForeignKey("Therapist")]
        public int? TherapistID { get; set; }

        [ForeignKey("Patient")]
        public int? PatientID { get; set; }

        [Required]
        [ForeignKey("Plan")]
        public int? planID { get; set; }

        [ForeignKey("MedicalService")]
        public int? medicalServiceID { get; set; }
        
        [DisplayName("Patient Fees Charge")]
        public decimal? feesOnpatient { get; set; }

        [DisplayName("Medical Provider Fees")]
        public decimal? feesOnMedicalService { get; set; } //add them to the db


        public virtual Receptionist Receptionist { get; set; } = null;
        
        public virtual Therapist Therapist { get; set; } = null;

        public virtual Patient Patient { get; set; } = null;

        public virtual Plan Plan { get; set; } = null;

        public virtual MedicalService MedicalService { get; set; } = null;

        public virtual HashSet<Payment> Payments { get; set; } = new HashSet<Payment>();
        
        public virtual HashSet<TherapySession> TherapySessions { get; set; } = new HashSet<TherapySession>();
    }

}
