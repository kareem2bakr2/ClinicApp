using System.ComponentModel;
using System.Threading.Channels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicApp.ViewModel
{

    public class AppointmentDetailsTherapist
    { 
        public int id { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
    }

    public class AppointmentDetailsViewModel
    {
        public DateTime CreationDate { get; set; }
        public int ID { get; set; }
        public AppointmentStatus Status { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [DataType(dataType: DataType.MultilineText)]
        public string Notes { get; set; }
        public decimal? Discount { get; set; }
        [Display(Name = "Discount Percentage")]
        public decimal? DiscountPercentage { get; set; }
        [Display(Name = "Final Price")]
        public decimal? FinalPrice { get; set; }

        [DisplayName("Reserved By")]
        public string ReceptName { get; set; }

        [DisplayName("Therapist Name")]
        public string TherapistName { get; set; }

        [DisplayName("Therapist Manager Name")]
        public string TherapistManagerName { get; set; }

        public int? PatientID { get; set; }

        [DisplayName("Patient Name")]
        public string PatientName { get; set; }

        public int? planID { get; set; }
        [DisplayName("Plan Name")]
        public string PlanName { get; set; }

        [DisplayName("Medical Service")]
        public string MedicalServiceName { get; set; }

        [DisplayName("Patient Fees")]
        public decimal? feesOnpatient { get; set; }


        [DisplayName("Payed Patient Fees")]
        public decimal? PayablefeesOnpatient { get; set; }

        [DisplayName("Medical Provider Fees")]
        public decimal? feesOnMedicalService { get; set; } //add them to the db

        public bool CancelBtnEnable { get; set; }

        public virtual List<AppointmentPaymentViewModel> Payments { get; set; }
            = new List<AppointmentPaymentViewModel>();
        public virtual List<AppointmentDetialsSessionViewModel> TherapySessions { get; set; }
            = new List<AppointmentDetialsSessionViewModel>();
        public List<AppointmentDetailsTherapist> allTherapists { get; set; }= new List<AppointmentDetailsTherapist>();

    }
    public class AppointmentPaymentViewModel
    {
        public int ID { get; set; }
        public bool IsCredit { get; set; }
        public decimal? Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public string ReceptName { get; set; }

        [DisplayName("Payed By")]
        public PaymentType PayedBy { get; set; }


    }
    public class AppointmentDetialsSessionViewModel {

        public int ID { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public SessionStatus SessionStatus { get; set; }
        public string Notes { get; set; }

    }

    public class CompleteAppointmentViewModel {

        public int ID { get; set; }
        [DisplayName("Medical Provider Percentage")]
        public decimal? MedicalProviderPercentage { get; set; }

        public DateOnly StartDate { get; set; }
        public List<DayTimeSessionViewModel> DayTimeSessions { get; set; } = new List<DayTimeSessionViewModel>();
        public int therapistId { get; set; }

    }
    public class DayTimeSessionViewModel { 
        public DayOfWeek Day { get; set; }
        
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
    public class AppointmentPayViewModel {

        public int ID { get; set; }
        [Required]
        public decimal Amount { get; set;  }
        [Required]
        public PaymentMethod method { get; set; }

        public string notes { get;set;  } 
    }

    public class AppointmentEditViewModel { 
        public int ID{ get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string notes { get; set; }
    }
}
