namespace ClinicApp.ViewModel
{
    public class PatientDetailsViewModel
    {
        public int ID { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }
        public string Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string EmergencyPhone { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyRelationShip { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? DOB { get; set; }
        public int? Age { get; set; }
        public string UserName { get; set; }
        public  HashSet<PatientAppointmentDetailsViewModel> Appointments { get; set; }
            = new HashSet<PatientAppointmentDetailsViewModel>();

    }
    public class PatientAppointmentDetailsViewModel
    {

        public int ID { get; set; }
        public AppointmentStatus? Status { get; set; }
        private DateOnly? _StartDate;
        public DateOnly? StartDate { get { return _StartDate; }
            set
            {
                if (value == DateOnly.FromDateTime(DateTime.MaxValue)) {
                    _StartDate = null;
                }
                else { _StartDate  =value; }
            } 
        }

        private DateOnly? _EndDate;
        public DateOnly? EndDate { get { return _EndDate; } 
            set
            {
                if (value == DateOnly.FromDateTime(DateTime.MaxValue)) { 
                    _EndDate = null;
                }else
                    _EndDate = value; 
            } }
        public string TherapistName { get; set; }
        public string medicalServiceName { get; set; }

    }

}
