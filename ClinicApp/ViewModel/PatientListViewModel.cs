namespace ClinicApp.ViewModel
{
    public class PatientListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string EmergencyPhone { get; set; }
        public string EmergencyContact { get; set; }
        public string Phone { get; set; }
        public TherapySession session { get; set; }

    }
}
