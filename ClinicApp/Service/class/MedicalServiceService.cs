using System.Diagnostics.Eventing.Reader;

namespace ClinicApp.Service
{
    public class MedicalServiceService : IMedicalServiceService
    {
        private readonly IMedicalServiceRepository medicalServiceRepo;

        public MedicalServiceService(IMedicalServiceRepository medicalServiceRepo) {
            this.medicalServiceRepo = medicalServiceRepo;
        }


        public async Task<MediaclServiceIndexViewModel> GetAllMedicalService(MediaclServiceIndexViewModel model) 
        {
            var items = medicalServiceRepo.GetAll();

            if (model.SearchName is not null) { 
             
                items = items.Where(x =>
                    x.ID.ToString().Contains(model.SearchName) ||
                    x.Name.Contains(model.SearchName) ||
                    x.Discount.ToString().Contains(model.SearchName));

            
            }
            if (model.ContractStatus == ContractStatus.Active)
            {
                items = items.Where(c => c.EndDateContract != null &&
                        c.EndDateContract >= DateOnly.FromDateTime(DateTime.Today));
            }else if (model.ContractStatus == ContractStatus.Expired){
                items = items.Where(c => c.EndDateContract != null &&
                        c.EndDateContract <= DateOnly.FromDateTime(DateTime.Today));
            }else if(model.ContractStatus == ContractStatus.Draft)
            {
                items = items.Where(c => c.EndDateContract == null );
            }
            model.Count = items.Count();

            model.items = items.Skip((model.PageIndex - 1) * 10 ).Take(10)
                .Select(x => new MedicalServiceViewModel{
                    id = x.ID,
                    Description = x.Description,
                    Discount = x.Discount,
                    EndDateContract = x.EndDateContract,
                    Name=x.Name,
                    StartDateContract = x.StartDateContract
                }
                ).ToList();
            model.PageIndex = model.PageIndex;
            return model;
        }

        public async Task<MedicalService> CreateMedicalServiceAsync(MedicalServiceViewModel model) {
            var medicalService = new MedicalService {
                EndDateContract = model.EndDateContract ,
                Name = model.Name ,
                Discount = model.Discount ,
                StartDateContract= model.StartDateContract ,
                Description = model.Description  
            };

            await medicalServiceRepo.AddAsync(medicalService);
            await medicalServiceRepo.SaveAsync();
            return medicalService;
        }

        public async  Task<MedicalServiceDetailsViewModel> GetMedicalServiceDetailsasync(MedicalServiceDetailsViewModel model)
        {
            var medicalService = await medicalServiceRepo.GetByIdAsync(model.id);
            if(medicalService is null ) return null;
            if (model.EndDatefilter is null || model.EndDatefilter == default || model.EndDatefilter == DateOnly.MinValue)
                model.EndDatefilter = DateOnly.MaxValue;
            var e1 = medicalService.Payments
                .Where(p => DateOnly.FromDateTime(p.Date) >= model.StartDatefilter &&
                         DateOnly.FromDateTime(p.Date) <= model.EndDatefilter)
                .ToList();
            return new MedicalServiceDetailsViewModel
            {
                id = medicalService.ID,
                StartDatefilter = model.StartDatefilter,
                EndDatefilter = model.EndDatefilter,
                Description = medicalService.Description,
                Discount = medicalService.Discount,
                EndDateContract = medicalService.EndDateContract,
                Name = medicalService.Name,
                StartDateContract = medicalService.StartDateContract,
                Payments = medicalService.Payments
                .Where(p=>DateOnly.FromDateTime(p.Date) >= model.StartDatefilter &&
                         DateOnly.FromDateTime( p.Date) <= model.EndDatefilter)
                .Select(p => new MedicalServicePayments
                {
                    ID = p.ID,
                    Amount = p.Amount,
                    AppointmentID = p.AppointmentID,
                    Date = p.Date,
                    IsCredit = p.IsCredit,
                    Method = p.Method,
                    Notes = p.Notes,
                    PaymentType = p.PaymentType
                }).ToList(),
                Appointments = medicalService.Appointments
                .Where(a=>DateOnly.FromDateTime(a.StartDate ?? DateTime.MaxValue )  >=  model.StartDatefilter &&
                    DateOnly.FromDateTime(a.StartDate ?? DateTime.MinValue) <= model.EndDatefilter
                )
                .Select(a => new MedicalServiceAppointment
                {
                    ID = a.ID,
                    Discount =a.Discount,
                    EndDate=a.EndDate,
                    feesOnMedicalService =a.feesOnMedicalService,
                    feesOnpatient = a.feesOnpatient,
                    PatientName = a.Patient?.Name,
                    StartDate = a.StartDate,
                    Status = a.Status
                })
                .ToList()
            };


        }

        public async Task<ReturnResult> EditMedicalServiceAsync(MedicalServiceViewModel model)
        {
            var ms = await medicalServiceRepo.GetByIdAsync(model.id);
            
            if (ms is null) return new ReturnResult {flag = false , message = "Can't Find Medical Service" };

            ms.Description = model.Description;
            ms.Name = model.Name;
            ms.EndDateContract = model.EndDateContract;
            await medicalServiceRepo.SaveAsync();
            
            return new ReturnResult { flag = true, message = "Changes Saved Successfully" };
        
        }


    }
}
