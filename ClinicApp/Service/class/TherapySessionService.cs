using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace ClinicApp.Service
{
    public class TherapySessionService : ITherapySessionService
    {
        private readonly ITherapySessionRepository therapySessionRepo;
        private readonly ITreatmentPlanRepository treatmentPlanRepo;
        private readonly ITherapistRepository therapistRepo;
        private readonly IPaymentRepository paymentRepo;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<ApplicationUser> userManager;
        public TherapySessionService(
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            ITherapySessionRepository therapySessionRepo ,
            ITreatmentPlanRepository tdb,
            ITherapistRepository therapistRepository,
            IPaymentRepository paymentRepo) 
        {
            this.currentUserService = currentUserService;
            this.userManager = userManager;
            this.paymentRepo = paymentRepo;
            this.therapistRepo = therapistRepository;
            this.treatmentPlanRepo = tdb;
            this.therapySessionRepo = therapySessionRepo;
        }
        public async Task<TherapySessionIndexModelView> GetAllIndexAsync(TherapySessionIndexModelView model) 
        {

            model.AllTreatmentPlanNames = treatmentPlanRepo.GetAll().Select(t => t.Name).ToList();
            model.AllTherapistNAmes = therapistRepo
                .GetAll()
                .Select(t=>new TherapySessionTherapistModelView {id = t.ID , name = t.Name }).ToList();
            

            var items = therapySessionRepo.GetAll();
            if(model.SearchName is not null)
                items = items.Where(e=>
                e.TreatmentPlan.Name.Contains(model.SearchName) ||
                e.SessionStatus.ToString().Contains(model.SearchName) ||
                e.Notes.Contains(model.SearchName) 
                );

            //filter based on account 
            var userId = currentUserService.CurrentUserID;
            var user = await userManager.FindByIdAsync(userId);

            if (user is null) return null;

            var isPatient = await userManager.IsInRoleAsync(user, ApplicationRole.Patient);
            var patientID = user.Patient?.ID;
            if (isPatient)
            {
                if (patientID is null) return null;
                items = items.Where(s => s.Appointment.PatientID == patientID);
            }

            var isTherapist = await userManager.IsInRoleAsync(user, ApplicationRole.Therapist);
            var therapistID = user.Therapist?.ID;
            if (isTherapist)
            {
                if (therapistID is null) return null;
                items = items.Where(s => s.Appointment.TherapistID == therapistID || s.TheeapistID == therapistID);
            }


            if (model.SessionDateFilter != default || model.SessionDateFilter != DateOnly.MinValue)
                items = items.Where(s=>s.SessionDate ==  model.SessionDateFilter);
            
            if (model.TherapistNameFilter.Any())
                items = items.Where(s=>model.TherapistNameFilter.Contains(s.TheeapistID ?? 0));
            
            if(model.SessionStatusFilter != null)
                items = items.Where(s=>s.SessionStatus == model.SessionStatusFilter);
            
            if(model.StartTimeFilter != default )
                items = items.Where(s=>s.StartTime == model.StartTimeFilter);
            if(model.EndTimeFilter != default )
                items = items.Where(s=>s.EndTime == model.EndTimeFilter);
            if (model.TreatmentPlanNameFilter is not null)
                items = items.Where(tp => tp.TreatmentPlan.Name == model.TreatmentPlanNameFilter);

            model.countPages = (int) Math.Ceiling(items.Count() /20.0) ;
            model.items = items
                .OrderByDescending(s => s.SessionDate)
                .ThenBy(s => s.StartTime)
                .Skip((model.PageNumber - 1) * 20)
                .Take(20).Select(ts=>new TherapySessionModelView {
                    ID = ts.ID ,
                    AppointmentID = ts.AppointmentID , 
                    EndTime = ts.EndTime , 
                    StartTime =ts.StartTime,
                    Notes = ts.Notes,
                    SessionDate = ts.SessionDate , 
                    SessionStatus = ts.SessionStatus ,
                    TheeapistID = ts.TheeapistID,
                    TheeapistName = ts.Therapist.Name,
                    TreatmentPlanID = ts.TreatmentPlanID,
                    TreatmentPlanName = ts.TreatmentPlan.Name})
                .ToList();
            return model;

        }
    
       public async Task<ReturnResult> CancelSessionAsync(int id)
        {
            var session = await therapySessionRepo.GetByIdAsync(id);

            session.Therapist = null;

            if (session.SessionStatus == SessionStatus.Completed) return new ReturnResult { flag = false, message = "Can't cancel session" };
            session.SessionStatus = SessionStatus.Cancelled;
            
            await therapySessionRepo.SaveAsync();
            
            return new ReturnResult { flag = true, message = "Session Canceled Successfully" };
        }

        public async Task<TherapySessionModelView> GetDetialsAsync(int id) 
        {
            // patient appointment patient id is the same 
            // therapist session therapist id is the same

            //get session
            var model = await therapySessionRepo.GetByIdAsync(id);
            if (model is null) return null;
            // check if authorized
            var userId = currentUserService.CurrentUserID;
            var user = await userManager.FindByIdAsync(userId);

            if (user is null) return null;

            if (await userManager.IsInRoleAsync(user, ApplicationRole.Patient) &&
                (user.Patient is null || user.Patient.ID != model.Appointment.PatientID)) return null;
            if(await userManager.IsInRoleAsync(user, ApplicationRole.Therapist) &&
                (user.Therapist is null || user.Therapist.ID != model.TheeapistID)) return null;

            return new TherapySessionModelView { 
                alltherapists = therapistRepo.GetAll().Select(t=>new TherapySessionTherapistModelView { id=t.ID,name=t.Name}).ToList(),
                ID= model.ID,
                AppointmentID = model.AppointmentID,
                EndTime = model.EndTime,
                Notes = model.Notes,
                SessionDate = model.SessionDate,
                SessionStatus = model.SessionStatus,
                StartTime = model.StartTime,
                TheeapistID = model.TheeapistID,
                TheeapistName = model.Therapist?.Name,
                TreatmentPlanID = model.TreatmentPlanID,
                TreatmentPlanName = model.TreatmentPlan?.Name
            };

        }

        public async Task<ReturnResult> CompleteSessionAsync(SessionCompleteViewModel model)
        {
            var session = await therapySessionRepo.GetByIdAsync(model.ID);
            if (session is null) return new ReturnResult { flag = false, message = "Failed to complete session" };

            session.SessionStatus = SessionStatus.Completed;
            session.SessionDate = model.SessionDate;
            session.StartTime = model.StartTime;
            session.EndTime = model.EndTime;
            session.TheeapistID= model.TheeapistID;
            session.Notes = model.Notes;
            
            if(session.Appointment is null ) return new ReturnResult { flag = false, message = "Failed to complete session" };

            bool HasSessions =  session.Appointment.TherapySessions.Where(s => s.SessionStatus == SessionStatus.Pending).Any();

            if(!HasSessions) {
                session.Appointment.EndDate = session.SessionDate.ToDateTime(TimeOnly.MinValue) ;
                session.Appointment.Status = AppointmentStatus.completed; 
            }
            
            if(session.Therapist is null) return new ReturnResult { flag = false, message = "Failed to complete session" };
            

            var payment = new Payment {
                Amount = session.Therapist.hourRate * (decimal) ((session.EndTime - session.StartTime).TotalMinutes / 60) ,
                TherapistID = session.TheeapistID,
                Date = DateTime.Now,
                IsCredit = true,
                Notes = session.ID + "Salary",
                PaymentType = PaymentType.TherapistSalary,
            };

            await paymentRepo.AddAsync(payment);

            await therapySessionRepo.SaveAsync();

            return new ReturnResult { flag = true, message = "Session Completed." };

        }

        public async Task<ReturnResult> MarkAsNoShowAsync(int id)
        {
            var session = await therapySessionRepo.GetByIdAsync(id);
            
            if (session is null) return new ReturnResult { flag = false, message = "Couldn't Complete Method" };

            session.TheeapistID = null;
            session.SessionStatus = SessionStatus.NoShow;
            await therapySessionRepo.SaveAsync();
            return new ReturnResult { flag = true, message = "Action Competed Successfully" };        
        
        }
   
        public async Task<ReturnResult> ReScheduleSessionAsync(SessionResheduleViewMdel model) {
            
            var session = await therapySessionRepo.GetByIdAsync(model.ID);
            if (session is null || session.Appointment is null)
                return new ReturnResult { flag = false  , message = "Can't Complete Action!"}; 
            
            var newSession = new TherapySession
            {
                AppointmentID = session.AppointmentID,
                EndTime = model.EndTime,
                Notes = $"Rescheduled Session for session # {session.ID}",
                StartTime = model.StartTime,
                TheeapistID = session.Appointment.TherapistID,
                SessionDate =model.SessionDate,
                SessionStatus = SessionStatus.Pending,
                TreatmentPlanID = session.TreatmentPlanID
            };
            
            await therapySessionRepo.AddAsync(newSession);
            await therapySessionRepo.SaveAsync();
            model.ID = newSession.ID;
            return new ReturnResult { flag = true, message = "Session Resheduled Successfully." }; 
               
        }
         
        
    }
}
