using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepo;
        private readonly ITherapistRepository therapistRepository;
        private readonly IMedicalServiceRepository medicalServiceRepository;
        private readonly IPlanRepository planRepository;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPaymentRepository paymentRepository;
        private readonly IPatientRepository patientRepository;
        public AppointmentService(
            IAppointmentRepository appointmentRepo,
            IMedicalServiceRepository medicalServiceRepository,
            ITherapistRepository therapistRepository,
            IPlanRepository planRepository,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            IPaymentRepository paymentRepository,
            IPatientRepository patientRepository) 
        {
            this.currentUserService = currentUserService;
            this.appointmentRepo = appointmentRepo;
            this.therapistRepository = therapistRepository;
            this.planRepository = planRepository;
            this.medicalServiceRepository = medicalServiceRepository;
            this.userManager = userManager;
            this.paymentRepository = paymentRepository;
            this.patientRepository = patientRepository;
        }


        public async Task<ReturnResult> CancelAppointmenAsync(int id)
        {
            var appointment = await appointmentRepo.GetByIdAsync(id);
            bool completedSession = appointment.
                                TherapySessions
                                .Where(s=>s.SessionStatus == SessionStatus.Completed)
                                .Any();
            if ( completedSession ) 
                return new ReturnResult{
                    flag = false ,
                    message ="Failed To Cancel Appointment after Completing Session"};

            if(appointment.MedicalService is not  null) 
            {
                var paymentMedicalService = new Payment
                {
                   Amount = appointment.feesOnMedicalService ?? 0,
                   AppointmentID = id,
                   Date = DateTime.Now,
                   IsCredit = true,/////////////////////////////////////////////////////////////////////
                   Method = PaymentMethod.BankTransfer,
                   PaymentType = PaymentType.Other,
                   MedicalServiceID = appointment.medicalServiceID??null,
                   Notes = $"Return Canceled Appointment with Id {id}."
                 };
                 await paymentRepository.AddAsync(paymentMedicalService);
            }

            decimal payedValue = appointment.Payments
            .Where(p => p.PaymentType == PaymentType.PatientAppointmentFee)
            .Sum(p => p.Amount);

            if (payedValue > 0)
            {
                var appUser = await userManager.FindByIdAsync(currentUserService.CurrentUserID);

                var paymentRecept = new Payment
                {
                    Amount = payedValue,
                    Date = DateTime.Now,
                    IsCredit = true,/////////////////////////////////////////////////////////////////////
                    Method = PaymentMethod.Cash,
                    ReceptID = appUser.Receptionist.ID,
                    PaymentType = PaymentType.Other,
                    Notes = $"Canceled Appointment with Id {id}."
                };

                var paymentpatient = new Payment
                {
                    Amount =  appointment.feesOnpatient?? 0 -  payedValue,
                    AppointmentID = id,
                    Date = DateTime.Now,
                    IsCredit = true ,/////////////////////////////////////////////////////////////////////
                    Method= PaymentMethod.Cash,
                    PaymentType= PaymentType.Other,
                    ReceptID = appUser.Receptionist.ID,
                    PatientID = appointment.PatientID,
                    Notes = $"Return Canceled Appointment with Id {id} Cash."
                };

                await paymentRepository.AddAsync(paymentRecept);
                await paymentRepository.AddAsync(paymentpatient);
                
                foreach (var session in appointment.TherapySessions) { 
                    session.SessionStatus = SessionStatus.Cancelled;     
                }
                appointment.EndDate = DateTime.Now;
                appointment.Status = AppointmentStatus.cancelled;
                await paymentRepository.SaveAsync();

                return new ReturnResult { flag = true, 
                    message = $"Appointment Canceled Successfully , Please Return {payedValue}$ to the Patient."};
            
            }
            foreach (var session in appointment.TherapySessions)
            {
                session.SessionStatus = SessionStatus.Cancelled;
            }
            appointment.EndDate = DateTime.Now;
            appointment.Status = AppointmentStatus.cancelled;

            await appointmentRepo.SaveAsync();

            return new ReturnResult { flag = true ,message = "Appointment Canceled Successfully"};
        }

        public async Task<ReturnResult> EditAppointmentAsync(AppointmentEditViewModel model)
        {
            var appointment = await appointmentRepo.GetByIdAsync(model.ID);
            
            if (appointment == null) { return new ReturnResult { flag = false, message = "Can't Find Appointment" }; }
            appointment.StartDate = model.startDate;
            appointment.EndDate = model.endDate;
            appointment.Notes = model.notes;

            await appointmentRepo.SaveAsync();
            return new ReturnResult
            {
                flag = true,
                message = "Appointment Saved Successfully"
            };

        }
        public async Task<ReturnResult> PayAppointmentByPatientAsync(AppointmentPayViewModel payInfo)
        {
            var appUser = await userManager.FindByIdAsync(currentUserService.CurrentUserID);
            //if (appUser.type is not AccountType.Receptionist) 
            //    return new ReturnResult {
            //        flag = false,
            //        message = "You Need to be a receptionist to pay for an appointment"
            //    };
            var paymentRecept = new Payment
            {
                Amount = payInfo.Amount,
                Date = DateTime.Now,
                IsCredit = false,/////////////////////////////////////////////////////////////////////
                Method = payInfo.method,
                ReceptID = appUser.Receptionist.ID,
                PaymentType = PaymentType.PatientAppointmentFee,
                Notes = $"recived Appointment {payInfo.ID}fees from patient. {payInfo.notes} "
            };
            
            var paymentpatient = new Payment
            {

                Amount = payInfo.Amount,
                AppointmentID = payInfo.ID,
                Date = DateTime.Now,
                IsCredit = false,/////////////////////////////////////////////////////////////////////
                Method = payInfo.method,
                PaymentType = PaymentType.PatientAppointmentFee,
                ReceptID = appUser.Receptionist.ID,
                Notes = $"Payed Appointment Fess. {payInfo.notes}"
            };

            await paymentRepository.AddAsync(paymentpatient);
            await paymentRepository.AddAsync(paymentRecept);
            await paymentRepository.SaveAsync();
            return new ReturnResult {
                flag = true , 
                message = $"Saved..."
            }; 
        }

        public  async Task<ReturnResult> ScheduleAppointmentAsync(CompleteAppointmentViewModel model)
        {
            var appointment = await appointmentRepo.GetByIdAsync(model.ID);

            if (appointment.Plan is null) return new ReturnResult { flag = false, message = "Can't Schedule appointment with out a plan!" };
            //create sessionm 
            //model.MedicalProviderPercentage
            var feesOnMedicalService = model.MedicalProviderPercentage/100 * appointment.FinalPrice; 
            var feesOnPatient = appointment.FinalPrice - feesOnMedicalService;

            
            appointment.feesOnMedicalService = feesOnMedicalService;
            appointment.feesOnpatient = feesOnPatient;
            appointment.TherapistID = model.therapistId;
            
            
            int count = 0;
            
            var sessionTime =
                new List<DayTimeSessionViewModel>();
            for (int i = (int)model.StartDate.DayOfWeek; i < (int)model.StartDate.DayOfWeek + 7; i++) {
                sessionTime.AddRange(model.DayTimeSessions.Where(e => (int)e.Day == i%7));
            }


            var fromdate = model.StartDate;
            bool endwhile = true;
            while (endwhile) {
                var startDay = model.StartDate.DayOfWeek;//30 sun
                
                foreach (var day in sessionTime)
                {
                    int diffDays = (int)(day.Day-startDay);
                    if (diffDays < 0) diffDays += 7;
                    appointment.TherapySessions.Add(new TherapySession {
                        EndTime = day.EndTime,
                        StartTime = day.StartTime,
                        TheeapistID = model.therapistId,
                        SessionDate = fromdate.AddDays(diffDays),
                        SessionStatus = SessionStatus.Pending
                    });
                    count++;
                    if (count == appointment.Plan.numberOfSession) { endwhile = false; break; }
                }
                fromdate = fromdate.AddDays(7);
            }
            appointment.Status = AppointmentStatus.scheduled;

            await appointmentRepo.SaveAsync();
            return new ReturnResult { flag = true, message = "Appointment Completed Successfully" };
         }
      
        public async Task<AppointmentDetailsViewModel> GetAppointmentDetailsAsync(int id)
        {
            var userID = currentUserService.CurrentUserID;
            var user = await userManager.FindByIdAsync(userID);
            var isPatient = await userManager.IsInRoleAsync(user, ApplicationRole.Patient);

            if (user == null) return null;

            var appointment = appointmentRepo.GetAppointmentById(id);

            var model = appointment
                .Select(x => new AppointmentDetailsViewModel 
                {
                    ID = x.ID,
                    CreationDate = x.CreationDate,
                    Discount = x.Discount,
                    EndDate = x.EndDate,
                    FinalPrice = x.FinalPrice,
                    Notes = x.Notes,
                    Status = x.Status,
                    DiscountPercentage = x.DiscountPercentage,
                    feesOnMedicalService = x.feesOnMedicalService,
                    feesOnpatient = x.feesOnpatient,
                    //when shedule edit fees based on medical service provider company fees percentage
                    StartDate = x.StartDate,
                    CancelBtnEnable = x.TherapySessions.Where(s=>s.SessionStatus == SessionStatus.Completed).Count() == 0 && x.Status !=AppointmentStatus.cancelled ,
                    PatientID = x.PatientID,
                    PatientName = x.Patient.Name,
                    planID = x.planID,
                    PlanName = x.Plan.Name,
                    MedicalServiceName = x.MedicalService.Name,
                    PayablefeesOnpatient = x.feesOnpatient - 
                    x.Payments.Where(p=>p.PaymentType == PaymentType.PatientAppointmentFee).Sum(p=>p.Amount),
                    ReceptName = x.Receptionist.Name,
                    TherapistName =x.Therapist.Name,
                    TherapistManagerName = x.Therapist.Manager.Name,
                    Payments = x.Payments.Select(p=>new AppointmentPaymentViewModel 
                    {
                        ID = p.ID,
                        Amount = p.Amount,
                        Date = p.Date,
                        IsCredit = p.IsCredit,
                        Method = p.Method,
                        Notes = p.Notes,
                        PayedBy = p.PaymentType,
                        ReceptName =p.Receptionist.Name,
                    }).ToList(),
            
                    
                    TherapySessions = x.TherapySessions.Where(s=>!s.isDeleted)
                    .Select(s=>new AppointmentDetialsSessionViewModel {
                        ID = s.ID,
                        EndTime = s.EndTime,
                        Notes =s.Notes,
                        SessionDate = s.SessionDate,
                        SessionStatus = s.SessionStatus,
                        StartTime = s.StartTime,

                    }).ToList()
                }).FirstOrDefault();
            if (isPatient && (user.Patient is null || user.Patient?.ID != model.PatientID)) return null;

            model.allTherapists = therapistRepository.GetAll().Select(t => new AppointmentDetailsTherapist {
                id = t.ID,
                Name = t.Name,
                phone = t.Phone
            }).ToList();
            
            return model;
        }

        public async Task<AppointmentIndexViewModel> GetAppointmentsAsync(FilterAppointmentListViewModel filter)
        {

            var appointments = appointmentRepo.GetAll();

            var userId = currentUserService.CurrentUserID;
            var user = await userManager.FindByIdAsync(userId);
            var ispathient = await userManager.IsInRoleAsync(user, ApplicationRole.Patient);

            if (ispathient)
            {
                appointments = appointments.Where(t => t.PatientID == user.Patient.ID);
            }


            if (filter.TherapistsNames.Any())
                appointments =  appointments.Where(e => filter.TherapistsNames.Contains(e.Therapist.Name));
            if (filter.MedicalServicesNames.Any())
                appointments = appointments.Where(e=> filter.MedicalServicesNames.Contains(e.MedicalService.Name));
            if(filter.PlanNames.Any())
                appointments = appointments.Where(e=> filter.PlanNames.Contains(e.Plan.Name));
            if(filter.appointmentStatuses.Any())
                appointments = appointments.Where(e=> filter.appointmentStatuses.Contains(e.Status));
            
            if (filter.StartDateFrom is not null)
                appointments = appointments.Where(e =>
                DateOnly.FromDateTime(e.StartDate ?? DateTime.MinValue)
                >=  filter.StartDateFrom);
            if (filter.StartDateTo is not null)
                appointments = appointments.Where(e =>
                DateOnly.FromDateTime(e.StartDate ?? DateTime.MaxValue)
                <= filter.StartDateTo);
            
            if (filter.EndDateFrom is not null)
                appointments = appointments.Where(e =>
                DateOnly.FromDateTime(e.EndDate ?? DateTime.MinValue)
                >= filter.EndDateFrom);
            if (filter.EndDateTo is not null)
                appointments = appointments.Where(e =>
                DateOnly.FromDateTime(e.EndDate ?? DateTime.MaxValue)
                <= filter.EndDateTo);
            
            if (filter.CreationDateFrom is not null)
                appointments = appointments.Where(e =>
                DateOnly.FromDateTime(e.CreationDate)
                >= filter.EndDateFrom);
            if (filter.CreationDateTo is not null)
                appointments = appointments.Where(e =>
                DateOnly.FromDateTime(e.CreationDate)
                <= filter.CreationDateTo);

            var model = await appointments.Select(e => new AppointmentListViewModel
            {   ID = e.ID,
                CreationDate = e.CreationDate,
                Discount = e.Discount,
                DiscountPercentage =e.DiscountPercentage,
                EndDate = e.EndDate == null ? null : DateOnly.FromDateTime(e.EndDate ?? DateTime.MinValue) ,
                FinalPrice = e.FinalPrice,
                medicalServiceName = e.MedicalService.Name,
                Notes = e.Notes,
                PatientID = e.PatientID,
                patientName = e.Patient.Name,
                planName= e.Plan.Name,
                ReceptName = e.Receptionist.Name,
                StartDate = e.StartDate == null ? null :DateOnly.FromDateTime(e.StartDate ?? DateTime.MinValue),
                Status = e.Status,
                TherapistName = e.Therapist.Name,
                
            }).OrderByDescending(e=>e.CreationDate)
            .ToListAsync();
            var medicalServiceNames = medicalServiceRepository.GetAll().Select(e => e.Name).ToList();
            var therapistsNames = therapistRepository.GetAll().Select(e => e.Name).ToList();
            var plansNames = planRepository.GetAll().Select(e => e.Name).ToList();

            return new AppointmentIndexViewModel { 
                appointments = model,
                AvailableMedicalServices = medicalServiceNames,
                AvailableTherapists = therapistsNames,
                AvailablePlans = plansNames,
                FilterAppointmentListViewModel = filter
            };

        }
        public async Task<AppointmentCreateViewModel> GetCreateStaticDataAsync(int patientid) 
        {
            var user = await userManager.FindByIdAsync(currentUserService.CurrentUserID);
 
            var model = new AppointmentCreateViewModel
            {
                Patientid = patientid == 0 ? null : patientid,
                AllMedicalService =
                    medicalServiceRepository
                    .GetAll()
                    .Select(a => new AppointmentCreateMedicalServiceViewModel
                    { ID = a.ID, Name = a.Name ,Discount = a.Discount}).ToList(),
               AllPlans = planRepository.GetAll().Select(p => new AppointmentCreatePlanViewModel 
                {ID = p.ID , Name = p.Name , numberOfSession = p.numberOfSession , Price = p.Price })
               .ToList(),
               
            };
            
            //if (user.type == AccountType.Receptionist) 
            {
                model.AllPatietns = patientRepository
                    .GetAll()
                    .Select(p=>new AppointmentCreatePatientViewModel { 
                    id = p.ID,
                    Name = p.Name ,
                    phone = p.Phone})
                    .ToList();
            }

            return model;
        
        }
        public async Task<Appointment> AddAppointmentAsync(AppointmentCreateViewModel model)
        {
            var appointment = new Appointment();

            var plan = await planRepository.GetByIdAsync( model.planID );
            appointment.planID = plan.ID;

            if (model.medicalServiceID is not null) 
            {
                var medical = await medicalServiceRepository.GetByIdAsync(model.medicalServiceID ?? 0);
                appointment.DiscountPercentage = medical.Discount;
                appointment.Discount = plan.Price * medical.Discount / 100 ;
                appointment.medicalServiceID = model.medicalServiceID;
            }
            appointment.FinalPrice = plan.Price - appointment.Discount;
            appointment.Notes = model.Notes;

            var user = await userManager.FindByIdAsync(currentUserService.CurrentUserID);


            appointment.PatientID = model.Patientid;
            if (user.type == AccountType.Receptionist)
            {
                appointment.ReceptID = user.Receptionist.ID;
                
            }

            appointment.Status = AppointmentStatus.pending;
            await appointmentRepo.AddAsync(appointment);
            await appointmentRepo.SaveAsync();
            return appointment;
        }
    
    }

}
