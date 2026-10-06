namespace ClinicApp.ViewModel
{
    public class TherapySessionIndexModelView
    {
        public string SearchName { get; set; }
        public int countPages { get; set; } = 1;
        public int PageNumber { get; set; } = 1;
        public DateOnly SessionDateFilter { get; set; }
        public TimeOnly StartTimeFilter { get; set; }
        public TimeOnly EndTimeFilter { get; set; }
        public SessionStatus? SessionStatusFilter { get; set; }
        public List<int> TherapistNameFilter { get; set; } = new List<int>();
        public string TreatmentPlanNameFilter { get; set; }
        public List<TherapySessionTherapistModelView> AllTherapistNAmes { get; set; } 
            = new List<TherapySessionTherapistModelView>();  
        public List<string> AllTreatmentPlanNames { get; set; } = 
            new List<string>();
        public List<TherapySessionModelView> items { get; set; }
            = new List<TherapySessionModelView>();
    }
    public class TherapySessionTherapistModelView {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class TherapySessionModelView 
    {
        public List<TherapySessionTherapistModelView> alltherapists { get; set; } =
            new List<TherapySessionTherapistModelView>();
        public int ID { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public SessionStatus SessionStatus { get; set; }
        public string Notes { get; set; }
        public int? TheeapistID { get; set; }
        public string TheeapistName { get; set; }
        public int? AppointmentID { get; set; }
        public int? TreatmentPlanID { get; set; }
        public string TreatmentPlanName { get; set; }
    
    }
}
