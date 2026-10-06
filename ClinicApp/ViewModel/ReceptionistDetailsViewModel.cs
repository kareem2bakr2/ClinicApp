namespace ClinicApp.ViewModel
{
    public class ReceptionistDetailsViewModel
    {
        public int ID { get; set; }
        public string PhotoURL { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public DateTime WorktimeFrom { get; set; }
        public DateTime WorkTimeTO { get; set; }
        public DayOfWeek Dayoff { get; set; }
        
        public  HashSet<ReceptionistPymentDetailsViewModel> Payments { get; set; } =
            new HashSet<ReceptionistPymentDetailsViewModel>();
        public  HashSet<ReceptionistAppointmentsDetailsViewModel> Appointments { get; set; } 
            = new HashSet<ReceptionistAppointmentsDetailsViewModel>();
        

    }
    public class ReceptionistPymentDetailsViewModel
    {
        public int ID { get; set; }
        public bool IsCredit { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

    }

    public class ReceptionistAppointmentsDetailsViewModel
    {
        public int ID { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FinalPrice { get; set; }
        public string PatientName { get; set; }
        public string medicalServiceName { get; set; }
        
    }

}

