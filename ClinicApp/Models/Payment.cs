using ClinicApp.Data;
using System.Runtime.Serialization;

namespace ClinicApp.Models
{
    public class Payment : ILogsAttribuite
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
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        [Required]
        public bool IsCredit { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(1000 , ErrorMessage = "Max 1000 characters allowed.")]
        public string Notes { get; set; }

        [ForeignKey("Appointment")]
        public int? AppointmentID { get; set; }

        [ForeignKey("Therapist")]
        public int? TherapistID { get; set; }
        
        [ForeignKey("Receptionist")] 
        public int? ReceptID { get; set; }
        
        public int? PatientID { get; set; }
        public int? MedicalServiceID { get; set; }
        public virtual MedicalService MedicalService { get; set; } = null;
        public virtual Patient Patient { get; set; } = null;
        public virtual Appointment Appointment { get; set; } = null;

        public virtual Therapist Therapist { get; set; } = null;

        public virtual Receptionist Receptionist { get; set; } = null;
    }
}
