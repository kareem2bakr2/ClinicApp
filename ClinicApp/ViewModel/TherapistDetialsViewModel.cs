namespace ClinicApp.ViewModel
{
    public class TherapistDetialsViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhotoURL { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public TherapistSpecialization Specialization { get; set; }
        public SeniorityLevel SeniorityLevel { get; set; }
        public decimal hourRate { get; set; }
        public string ManagerName { get; set; }
        public  HashSet<TherapistDetialsAppointments> Appointments { get; set; }
                            = new HashSet<TherapistDetialsAppointments>();
        public  HashSet<TherapistDetialsTherapySessions> TherapySessions { get; set; }
                            = new HashSet<TherapistDetialsTherapySessions>();
        public  HashSet<TherapistDetialsPayments> Payments { get; set; } 
                            = new HashSet<TherapistDetialsPayments>();


    }
    public class TherapistDetialsAppointments 
    {
        public int ID { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
        public int? PatientID { get; set; }
        public string patientName { get; set; }
        public int? planID { get; set; }
        public string PlanName { get; set; }
    }

    public class TherapistDetialsTherapySessions
    {
        public int ID { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public SessionStatus SessionStatus { get; set; }
        public string Notes { get; set; }
        public int? TreatmentPlanID { get; set; }
        public string TreatmentPLanName { get; set; }

    }

    public class TherapistDetialsPayments
    {
        public int ID { get; set; }
        public bool IsCredit { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }

}
