using System.ComponentModel;

namespace ClinicApp.ViewModel
{
    public class MediaclServiceIndexViewModel
    {
        public List<MedicalServiceViewModel> items {  get; set; } 
            = new List<MedicalServiceViewModel>();
        public string SearchName { get; set; }
        public int PageIndex { get; set; } = 1;
        public int Count { get; set; } = 1;

        public ContractStatus? ContractStatus { get; set; }
    }

    public class MedicalServiceViewModel
    {
        public int id { get; set; }
        [Required]
        [Length(3, 100, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Discount must be a positive value.")]
        public decimal Discount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date Contract")]
        public DateOnly StartDateContract { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date Contract")]
        public DateOnly? EndDateContract { get; set; }
    }
    public class MedicalServiceDetailsViewModel
    {
        public DateOnly? StartDatefilter { get; set; }
            //= DateOnly.MinValue;
        public DateOnly? EndDatefilter { get; set; } 
            //= DateOnly.MaxValue;

        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }
        public DateOnly StartDateContract { get; set; }
        public DateOnly? EndDateContract { get; set; }
        public List<MedicalServiceAppointment> Appointments { get; set; }
                = new List<MedicalServiceAppointment>();
        public List<MedicalServicePayments> Payments { get; set; }
            = new List<MedicalServicePayments>();
        
    }



    public class MedicalServicePayments 
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsCredit { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int? AppointmentID { get; set; }
    }
    

    public class MedicalServiceAppointment 
    {

        public int ID { get; set; }

        public AppointmentStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Discount { get; set; }
        public string PatientName { get; set; }
        public decimal? feesOnpatient { get; set; }
        public decimal? feesOnMedicalService { get; set; } //add them to the db

    }
}