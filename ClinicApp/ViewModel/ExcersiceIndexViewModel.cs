using System.Globalization;

namespace ClinicApp.ViewModel
{
    public class ExcerciseIndexViewModel
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public int totalPageCount { get; set; } = 1;
        public string ssearchName { get; set; }
        public int NumberOfSetsMinFilter { get; set; } = 0;
        public int NumberOfSetsMaxFilter { get; set; } = 1000;
        public ExerciseDifficulty? difficultyFilter { get; set; }
        public List<ExerciseListModelView> items { get; set; } 
            = new List<ExerciseListModelView>();
    }
    public class ExerciseListModelView {
        public int ID { get; set; }
        public string Name { get; set; }
        public int NumberOfSets { get; set; }
        public ExerciseDifficulty Difficulty { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }

    }
    public class ExerciseCreateModelView
    {
        public int ID { get; set; } // in case of edit only 
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Url)]
        public string VideoURL { get; set; }
        public int NumberOfSets { get; set; }
        
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Instructions { get; set; }
        public ExerciseDifficulty Difficulty { get; set; }

        public int? CategoryID { get; set; }

        public List<SelectListModelView> allCategories { get; set; }
            = new List<SelectListModelView>();

        public List<int> SelectedEquipments { get; set; }
            = new List<int>();

        public List<SelectListModelView> allEquipmetns { get; set; }
            = new List<SelectListModelView>();
        public List<int> SelectedMuscles { get; set; }
                    = new List<int>();

        public List<SelectListModelView> allMuscles { get; set; }
            = new List<SelectListModelView>();
        
    }
    
    public class ExerciseDetailsModelView 
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoURL { get; set; }
        public int NumberOfSets { get; set; }
        public string Instructions { get; set; }
        public ExerciseDifficulty Difficulty { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<SelectListModelView> SelectedEquipments =
              new List<SelectListModelView>();

        public List<SelectListModelView> SelectedMuscles =
              new List<SelectListModelView>();
        public List<SelectListModelView> SelectedTreatmentPlans =
                      new List<SelectListModelView>();

        public List<SelectListModelView> allcategories { get; set; }
            = new List<SelectListModelView>();
        public List<SelectListModelView> allMuscles { get; set; }
            = new List<SelectListModelView>();
        public List<SelectListModelView> allEquipmetns { get; set; }
            = new List<SelectListModelView>();


    }


    public class SelectListModelView
    {
        public int? ID { get; set; }
        public string Name { get; set; }
    }


}
