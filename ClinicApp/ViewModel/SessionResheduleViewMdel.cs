namespace ClinicApp.ViewModel
{
    public class SessionResheduleViewMdel
    {
        public int ID { get; set;  }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

    }
}
