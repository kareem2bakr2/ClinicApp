using System.ComponentModel;

namespace ClinicApp.ViewModel
{
    public class PlanIndexModelView
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int PageCount { get; set;} = 1;
        
        public List<PlanListModelView> _items { get; set; } 
            = new List<PlanListModelView>();
    }

    public class PlanListModelView
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required] 
        [Range(1, 100, ErrorMessage = "Please Enter a valid number")]
        [DisplayName("Number of sessions")]
        public int? numberOfSession { get; set; }
        [Required]
        public decimal? Price { get; set; }
    }

    public class PlanDetailsModelView
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        [Range(1,100 , ErrorMessage = "Please Enter a valid number")]
        [DisplayName("Number of sessions")]
        public int? numberOfSession { get; set; }
        
        
        [Required]
        public decimal? Price { get; set; }
        public List<PlanAppointmentDetailsModelView> _items { get;set; } 
            = new List<PlanAppointmentDetailsModelView>();
    }
    public class PlanAppointmentDetailsModelView {
        public int ID { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
     
    }

}
