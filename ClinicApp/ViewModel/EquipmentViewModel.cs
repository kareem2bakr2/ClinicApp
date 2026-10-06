namespace ClinicApp.ViewModel
{
    public class ItemIndexViewModel
    {
        public IEnumerable<ItemViewModel> Items { get; set; } = new List<ItemViewModel>();
        public string SearchTerm { get; set; }
        public int pageNumber { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
    }

    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal price { get; set; }
        public string MerchantName { get; set; }
    }

    public class EquipmentViewModel

    {

        //details
        public int Id { get; set; }

        [Required(ErrorMessage = "Equipment name is required.")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000, ErrorMessage = "Price must be greater than zero.")]
        public decimal price { get; set; }

        [Required(ErrorMessage = "Merchant name is required.")]
        [MaxLength(100)]
        [Display(Name = "Merchant Name")]
        public string MerchantName { get; set; }
        //create , edit

        public List<int> SelectedExercises { get; set; } = new List<int>();

        public List<EquipmentExerciseViewModel> excersices { get; set; }
            = new List<EquipmentExerciseViewModel>();

    }
    public class EquipmentExerciseViewModel {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ExerciseDifficulty Difficulty { get; set; }
        public string CategoryName { get; set; }

    }
}
