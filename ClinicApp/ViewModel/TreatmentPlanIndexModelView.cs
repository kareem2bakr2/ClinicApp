namespace ClinicApp.ViewModel
{
    public class TreatmentPlanIndexModelView
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int PauseCount { get; set; } = 1;
        public List<TreatmentPlanModelview> _items { get; set; } 
            = new List<TreatmentPlanModelview>();

    }

    public class TreatmentPlanModelview {
        public int ID { get; set; }
        public string Name { get; set; }
        public TreatmentPlanCategory Category { get; set; }
        public TreatmentPlanLevel Level { get; set; }
        public string Notes { get; set; }

        public List<TreatmentPlanLinkModelview> Exercises { get; set; } = new List<TreatmentPlanLinkModelview>();
        public List<TreatmentPlanLinkModelview> AllExercises { get; set; } = new List<TreatmentPlanLinkModelview>();

    }
    public class TreatmentPlanLinkModelview { 
        public int ID { get; set; }
        public string Name { get; set; }
    
    }
    public class TreatmentPlanCreateModelview
    {
        public int ID { get; set; }
        [Required]
        [MinLength(3 , ErrorMessage = "Name minimum Length is 3.")]
        public string Name { get; set; }
        [Required]
        public TreatmentPlanCategory Category { get; set; }
        [Required]
        public TreatmentPlanLevel Level { get; set; }
        
        [DataType(DataType.MultilineText)]
        [MaxLength(1000,ErrorMessage ="Maximum Length is 1000.")]
        public string Notes { get; set; }

    }

}
