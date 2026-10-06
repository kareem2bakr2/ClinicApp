namespace ClinicApp.ViewModel
{
    public class TherapistAddToMyTeamModelView
    {
        public int ID { get; set; }
         public string Name { get; set; }
        public string PhotoURL { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public TherapistSpecialization Specialization { get; set; }
    }
}
