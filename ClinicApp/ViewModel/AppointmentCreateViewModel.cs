using System.ComponentModel;

namespace ClinicApp.ViewModel
{
    public class AppointmentCreateViewModel
    {
        public int? Patientid {  get; set; }


        [MaxLength(1000,ErrorMessage ="Notes are too long")]
        public string Notes { get; set; }

        public decimal? Discount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        
        [Display(Name = "Final Price")]
        public decimal? FinalPrice { get; set; }

        
        [Required]
        public int planID { get; set; }

        public int? medicalServiceID { get; set; }
        public List<AppointmentCreateMedicalServiceViewModel> AllMedicalService { get; set; }
    = new List<AppointmentCreateMedicalServiceViewModel>();
        public List<AppointmentCreatePatientViewModel> AllPatietns { get; set; }
            = new List<AppointmentCreatePatientViewModel>();
        public List<AppointmentCreatePlanViewModel> AllPlans { get; set; }
            = new List<AppointmentCreatePlanViewModel>();

    }
   
    public class AppointmentCreateMedicalServiceViewModel 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }


    }
    public class AppointmentCreatePlanViewModel 
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Display(Name = "Number Of Sessions")]
        public int? numberOfSession { get; set; }

        [DataType(dataType: DataType.Currency)]
        public decimal? Price { get; set; }

    }
    public class AppointmentCreatePatientViewModel 
    {
        public int id { get; set; }
        public string Name { get; set;  }
        public string phone { get; set; }

    }

}
