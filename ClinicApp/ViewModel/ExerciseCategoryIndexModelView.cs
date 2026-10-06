namespace ClinicApp.ViewModel
{
    public class ExerciseCategoryIndexModelView
    {
        public int PageNumber { get; set; }=1;
        public int PagesCount { get; set; } = 1;
        public int PageSize { get;set; } = 10; 
        public string searchName { get; set; }
        
        public List<ExerciseCategoryModelView> _items { get; set; }
                        = new List<ExerciseCategoryModelView>();
        
        
    }

    public class ExerciseCategoryModelView {

        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class ExerciseCategoryDetailsModelView
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<ExerciseLinks> _items { get; set; } 
            = new List<ExerciseLinks>(); 
    }

    public class ExerciseLinks { 
    
        public int Id { get; set; }
        public string Displayname { get; set; }
    }
}
