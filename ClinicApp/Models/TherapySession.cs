using ClinicApp.Data;
using System.Net.NetworkInformation;

namespace ClinicApp.Models
{
    public class TherapySession : ILogsAttribuite
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

        [DataType(DataType.Date)]
        [Required]
        [Display(Name ="Session Date")]
        public DateOnly SessionDate { get; set; }

        [DataType(DataType.Time)]
        [Required]
        [Display(Name = "Start Sesssion Time")]
        public TimeOnly StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Sesssion Time")]
        public TimeOnly EndTime { get; set; }

        [Required]
        [Display(Name = "Session Status")]
        public SessionStatus SessionStatus { get; set; }
        
        [MaxLength(1000 ,ErrorMessage = "Maximum Note Length is 1000 Chars")]
        public string Notes { get; set; }

        [ForeignKey("Therapist")]
        public int? TheeapistID { get; set; }

        [ForeignKey("Appointment")]
        public int? AppointmentID { get; set; }

        [ForeignKey("TreatmentPlan")]
        public int? TreatmentPlanID { get; set; }
        public virtual Appointment Appointment { get; set; } = null;

        public virtual TreatmentPlan TreatmentPlan { get; set; } = null;

        public virtual Therapist Therapist { get; set; } = null;
    }
}
