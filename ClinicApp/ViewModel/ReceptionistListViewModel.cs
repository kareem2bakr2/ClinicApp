namespace ClinicApp.ViewModel
{
    public class ReceptionistListViewModel
    {
        public int ID { get; set; }
        public string PhotoURL { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime WorktimeFrom { get; set; }
        public DateTime WorkTimeTO { get; set; }
        public DayOfWeek Dayoff { get; set; }
    }
}
