namespace ClinicApp.ViewModel
{
    public class MuscleIndexViewModel
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public int totalPages { get; set; }
        public List<MuscleViewModel> _items { get; set; } = new();
    }

    public class MuscleViewModel { 
        public int id { get; set; }
        public string name { get; set; }
        public List<ExerciseLinks> _exercises { get; set; } = new();  
        
    }

    public class ExercisesLinks { 
        public int id { get; set; }
        public string DisplayName {  get; set; }
    
    }


}
