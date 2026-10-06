namespace ClinicApp.ViewModel
{
    public class TherapistListViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoURL { get; set; }
        public TherapistSpecialization Specialization { get; set; }
        public SeniorityLevel SeniorityLevel { get; set; }
        public decimal hourRate { get; set; }

    }
}
