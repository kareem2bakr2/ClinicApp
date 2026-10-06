namespace ClinicApp.ViewModel
{
    public class PaymentIndexModelView
    {
        public int pageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
        public int pageCount { get; set; } = 1;
        public string SearchWord { get; set; }
        public DateOnly? FromDate {  get; set; }
        public DateOnly? ToDate {  get; set; }
        public PaymentMethod? PaymentMethodFilter { get; set; }
        public PaymentType? PaymentTypeFilter { get; set; }
        public Paymentfilter? IsCreditFilter { get; set; } // selct box of cridit and debit 
        
        public List<int> TherpaistSIDFilter { get; set; } 
            = new List<int>();  


        public List<int> ReciptIDsFilter { get; set; }
            = new List<int>();
        public List<int> PatientIDsFilter { get; set; }
                    = new List<int>();
        public List<int> medicalServiceIDsFilter { get; set; }
            = new List<int>();

        public List<DisplayFilter> allTherapists { get; set; }
            = new List<DisplayFilter>();
        public List<DisplayFilter> allPatients { get; set; }
                    = new List<DisplayFilter>();

        public List<DisplayFilter> allRecipts { get; set; }
            = new List<DisplayFilter>();
        public List<DisplayFilter> allMedicalService { get; set; }
            = new List<DisplayFilter>();

        public List<PaymentListModelView> _items = 
                new List<PaymentListModelView>();
       
    }
    public class DisplayFilter{
        public int id { get; set; }
        public string nametag { get; set; }
    }

    public class PaymentListModelView {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsCredit { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int? AppointmentID { get; set; }
        public int? TherapistID { get; set; }
        public string TherapistName { get; set; }
        public int? ReceptID { get; set; }
        public string ReceptName { get; set; }
        public int? PatientID { get; set; }
        public string PatientName { get; set; }
        public int? MedicalServiceID { get; set; }
        public string medicalServiceName { get; set; }
    }
}
