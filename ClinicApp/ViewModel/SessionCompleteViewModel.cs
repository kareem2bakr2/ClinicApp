namespace ClinicApp.ViewModel
{
    public class SessionCompleteViewModel
    {
        public int ID { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int TheeapistID { get; set; }
        public string Notes { get; set; }
    }
}
