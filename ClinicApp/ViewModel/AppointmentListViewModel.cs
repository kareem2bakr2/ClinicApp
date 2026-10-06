namespace ClinicApp.ViewModel
{
    public class AppointmentIndexViewModel {
        public List<AppointmentListViewModel> appointments { get; set; } = new List<AppointmentListViewModel>();

        // Dropdown selection options populated by Controller
        public List<string> AvailableTherapists { get; set; } = new List<string>();
        public List<string> AvailableMedicalServices { get; set; } = new List<string>();
        public List<string> AvailablePlans { get; set; } = new List<string>();

        public FilterAppointmentListViewModel FilterAppointmentListViewModel { get; set; }


    }
    public class AppointmentListViewModel
    {
        public int ID { get; set; }
        public AppointmentStatus Status { get; set; } 
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Notes { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FinalPrice { get; set; }
        public string ReceptName { get; set; }
        public string TherapistName { get; set; }
        public int? PatientID { get; set; }
        public string patientName { get; set; } // search by text by js search bar, is clickable to his details by id
        public string planName { get; set; }
        public string medicalServiceName { get; set; }


    }
    public class FilterAppointmentListViewModel {

        public List<string> TherapistsNames { get; set; } = new List<string>();
        public List<string> MedicalServicesNames { get; set; } = new List<string>();
        public List<string> PlanNames { get; set; } = new List<string>();
        public List<AppointmentStatus> appointmentStatuses { get; set; }= new List<AppointmentStatus>();  
        public DateOnly? StartDateFrom { get; set; }
        public DateOnly? StartDateTo { get; set; }
        public DateOnly? EndDateFrom { get; set; }
        public DateOnly? EndDateTo { get; set; }
        public DateOnly? CreationDateFrom { get; set; }
        public DateOnly? CreationDateTo { get; set; }

    }
}
