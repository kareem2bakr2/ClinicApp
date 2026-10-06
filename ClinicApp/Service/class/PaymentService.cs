namespace ClinicApp.Service
{
    public class PaymentService :IPaymentService
    {
        private readonly IPaymentRepository paymentRepo;
        private readonly ITherapistRepository therapistRepo;
        private readonly IPatientRepository patientRepo;
        private readonly IMedicalServiceRepository medicalServiceRepo;
        private readonly IReceptionistRepository receptionistRepo;
        public PaymentService(
            IPaymentRepository paymentRepo,
            IReceptionistRepository receptionistRepo,
            ITherapistRepository therapistRepo,
            IPatientRepository patientRepo,
            IMedicalServiceRepository medicalServiceRepo
        ){
            this.medicalServiceRepo =medicalServiceRepo;
            this.receptionistRepo = receptionistRepo;
            this.therapistRepo =therapistRepo;
            this.patientRepo =patientRepo;
            this.paymentRepo = paymentRepo;
        }
        public async Task<PaymentIndexModelView> GetAllAsync(PaymentIndexModelView filter) 
        {
            var result = new PaymentIndexModelView {
                pageSize = filter.pageSize,
                PageNumber = filter.PageNumber,
            };

            var items = paymentRepo.GetAll();
            if (filter.IsCreditFilter.HasValue)
            {
                if (filter.IsCreditFilter == Paymentfilter.credit)
                {
                    items = items.Where(x => x.IsCredit == true);
                }
                else if (filter.IsCreditFilter == Paymentfilter.debit)
                {
                    items = items.Where(x => x.IsCredit == false);
                }
            }
            if (filter.FromDate is not null && filter.FromDate != default)
                items = items.Where(p => DateOnly.FromDateTime(p.Date) >= filter.FromDate);
            if (filter.ToDate is not null && filter.ToDate != default)
                items = items.Where(p => DateOnly.FromDateTime(p.Date) <= filter.ToDate);
            if (filter.PaymentMethodFilter is not null)
                items = items.Where(p => p.Method == filter.PaymentMethodFilter);
            if (filter.PaymentTypeFilter is not null)
                items = items.Where(p => p.PaymentType == filter.PaymentTypeFilter);
            if(filter.SearchWord is not null)
                items = items.Where(p => 
                    p.Date.ToString().Contains(filter.SearchWord) ||
                    p.Receptionist.Name.Contains(filter.SearchWord) ||
                    p.Therapist.Name.Contains(filter.SearchWord)||
                    p.Notes.Contains(filter.SearchWord)||
                    p.Amount.ToString().Contains(filter.SearchWord)||
                    p.MedicalService.Name.Contains(filter.SearchWord) ||
                    p.PaymentType.ToString().Contains(filter.SearchWord)||
                    p.Method.ToString().Contains(filter.SearchWord)
                );

            if (filter.TherpaistSIDFilter != null && filter.TherpaistSIDFilter.Any())
                items = items.Where(p => p.TherapistID.HasValue && filter.TherpaistSIDFilter.Contains(p.TherapistID.Value));

            if (filter.PatientIDsFilter != null && filter.PatientIDsFilter.Any())
                items = items.Where(p => p.PatientID.HasValue && filter.PatientIDsFilter.Contains(p.PatientID.Value));

            if (filter.medicalServiceIDsFilter != null && filter.medicalServiceIDsFilter.Any())
                items = items.Where(p => p.MedicalServiceID.HasValue && filter.medicalServiceIDsFilter.Contains(p.MedicalServiceID.Value));

            if (filter.ReciptIDsFilter != null && filter.ReciptIDsFilter.Any())
                items = items.Where(p => p.ReceptID.HasValue && filter.ReciptIDsFilter.Contains(p.ReceptID.Value));



            result.allMedicalService = medicalServiceRepo
                .GetAll()
                .Select(a => new DisplayFilter { nametag = a.Name, id = a.ID })
                .ToList();
            result.allPatients = patientRepo
                .GetAll()
                .Select(m => new DisplayFilter { id = m.ID, nametag = m.Name })
                .ToList();
            result.allTherapists =
                therapistRepo
                .GetAll()
                .Select(t => new DisplayFilter { id = t.ID, nametag = t.Name })
                .ToList();
            result.allRecipts = receptionistRepo.GetAll()
                .Select(r => new DisplayFilter { id = r.ID, nametag = r.Name })
                .ToList();

            result.pageCount = (int)Math.Ceiling( (decimal)items.Count() / filter.pageSize);
            result._items = items
                .Select(p=>new PaymentListModelView 
                {   ID=p.ID,
                    Amount =p.Amount,
                    AppointmentID =p.AppointmentID,
                    Date = p.Date,
                    IsCredit =p.IsCredit,
                    MedicalServiceID=p.MedicalServiceID,
                    medicalServiceName =p.MedicalService.Name,
                    Method = p.Method,
                    Notes =p.Notes,
                    PatientID =p.PatientID,
                    PatientName =p.Patient.Name,
                    PaymentType = p.PaymentType,
                    ReceptID =p.ReceptID,
                    ReceptName =p.Receptionist.Name,
                    TherapistID =p.TherapistID,
                    TherapistName =p.Therapist.Name

                })
                .OrderByDescending(r => r.Date)
                .Skip( (filter.PageNumber-1) * filter.pageSize )
                .Take(filter.pageSize)
                .ToList();
            result.PatientIDsFilter = filter.PatientIDsFilter;
            result.medicalServiceIDsFilter = filter.medicalServiceIDsFilter;
            result.ReciptIDsFilter = filter.ReciptIDsFilter;
            result.PaymentTypeFilter = filter.PaymentTypeFilter;
            result.PaymentMethodFilter = filter.PaymentMethodFilter;
            result.TherpaistSIDFilter = filter.TherpaistSIDFilter;
            result.FromDate = filter.FromDate;
            result.ToDate = filter.ToDate;
            result.SearchWord = filter.SearchWord;
            result.IsCreditFilter = filter.IsCreditFilter;
            
            return result;
        }


    }
}
