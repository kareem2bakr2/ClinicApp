using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Service
{

    public class TherapistService : ITherapistService

    {
        private readonly ITherapistRepository TherapistRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TherapistService(
            ITherapistRepository TherapistRepo,
            UserManager<ApplicationUser> _userManager,
            IWebHostEnvironment webHostEnvironment
            ) 
        {
            this._userManager = _userManager;
            this.TherapistRepo = TherapistRepo;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<TherapistListViewModel>> GetMyStaffTherapistsAsync(int managerid)
        {
            var therapists = await TherapistRepo.GetMyStaffTherapistsAsync(managerid);
            var therapistsViewModel = new List<TherapistListViewModel>() ;
            foreach (var therapist in therapists) {

                therapistsViewModel.Add(new TherapistListViewModel() { 
                    ID = therapist.ID,
                    Name = therapist.Name,
                    Email = therapist.Email,
                    Phone = therapist.Phone,
                    Specialization = therapist.Specialization,
                    SeniorityLevel = therapist.SeniorityLevel,
                    hourRate = therapist.hourRate,
                    PhotoURL = therapist.PhotoURL
                });
                
            }
            return therapistsViewModel;
        }

        public async Task<List<TherapistAddToMyTeamModelView>> GetTherapistWithNoMGR(int id) {
            var therapists = await TherapistRepo.GetTherapistWithNoMGR();
            therapists = therapists.Where(e => e.ID != id);
            var therapistsModelView = new List<TherapistAddToMyTeamModelView>();
            foreach (var therapist in therapists) {
                therapistsModelView.Add(new TherapistAddToMyTeamModelView()
                {
                    ID = therapist.ID,
                    Name = therapist.Name,
                    Email= therapist.Email,
                    Phone= therapist.Phone,
                    PhotoURL = therapist.PhotoURL,
                    Specialization = therapist.Specialization

                });
            }
            return therapistsModelView;
        }

        public async Task ChangeTherapistTeam(int[] TherapistsIdToTransfer, string managerAppID) 
        {
            var ManagerTherapist = await TherapistRepo.GetTherapistFromAppIdAsync(managerAppID);
            if (ManagerTherapist is null) { throw new Exception("Therapist Not Found"); }
            //chack manager has role to add another therapist under theier team

            foreach (var id in TherapistsIdToTransfer) 
            {
                var therapist =await TherapistRepo.GetByIdAsync(id);
                if (therapist == null) throw new Exception("Wrong Therapist Id");
                if (therapist.isDeleted) throw new Exception("Therapist Has Been Deleted Before ");
                if (therapist.ManagerID is null || therapist.ManagerID == 0)
                {
                    therapist.ManagerID = ManagerTherapist.ID;
                }
                else {
                 throw new Exception("Therapist Has Manager.");
                }
            }
            await TherapistRepo.SaveAsync();
            
        }

        public async Task removeFromTeamAsync(int id, int managerid)
        {
            var therapist = await TherapistRepo.GetByIdAsync(id);
            if (therapist.ManagerID == managerid)
                therapist.ManagerID = null;
            await TherapistRepo.SaveAsync();

        }

        public async Task<TherapistDetialsViewModel> GetTherapistDetialsAsync(int id)
        {
            var Therapist = await TherapistRepo.GetTherapistWithAppointmentsSessionsPaymentAsync(id);
            var payments = new HashSet<TherapistDetialsPayments>();
            var Appointments = new HashSet<TherapistDetialsAppointments>();
            var Sessions = new HashSet<TherapistDetialsTherapySessions>();
            if (Therapist is null) return null;
            var paymentsDB = Therapist.Payments
                                      .Where(p => p.Date >  DateTime.Today.AddMonths(-3))
                                      .OrderByDescending(p=>p.Date);
            var AppointmentsDB = Therapist.Appointments
                                          .Where(p => p.StartDate > DateTime.Today.AddMonths(-6) || p.StartDate is null)
                                          .OrderByDescending(p=>p.StartDate);
            var SessionsDB = Therapist.TherapySessions
                                       .Where(p=>p.SessionDate >DateOnly.FromDateTime( DateTime.Today.AddMonths(-3)))
                                       .OrderByDescending(e=>e.SessionDate);

            foreach (var payment in paymentsDB) {
                payments.Add(new TherapistDetialsPayments {
                    ID = payment.ID,
                    Date = payment.Date,
                    IsCredit = payment.IsCredit,
                    Method = payment.Method,
                    Notes = payment.Notes
                });
            }

            foreach (var Appointment in AppointmentsDB)
            {
                Appointments.Add(new TherapistDetialsAppointments
                {
                    ID = Appointment.ID,
                    PatientID = Appointment.PatientID,
                    patientName= Appointment.Patient.Name,
                    planID=Appointment.planID,
                    PlanName= Appointment.Plan.Name,
                    StartDate = Appointment.StartDate,
                    EndDate = Appointment.EndDate,
                    Notes= Appointment.Notes,
                    Status = Appointment.Status

                });
            }

            foreach (var session in SessionsDB)
            {
                Sessions.Add(new TherapistDetialsTherapySessions
                {
                    ID = session.ID,
                    SessionDate = session.SessionDate,
                    SessionStatus = session.SessionStatus,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    Notes = session.Notes,
                    TreatmentPlanID = session.TreatmentPlanID,
                    TreatmentPLanName = session.TreatmentPlan?.Name,
                });
            }
            return new TherapistDetialsViewModel 
            {
                ID = Therapist.ID,
                Name = Therapist.Name,
                Appointments = Appointments,
                Email = Therapist.Email,
                Gender = Therapist.Gender,
                hourRate  =Therapist.hourRate,
                ManagerName = Therapist.Manager?.Name,
                Payments = payments,
                Phone = Therapist.Phone,    
                PhotoURL =Therapist.PhotoURL,
                SeniorityLevel = Therapist.SeniorityLevel,
                Specialization = Therapist.Specialization,
                TherapySessions = Sessions 
            };
        }

        public async Task<TherapistEditViewModel> GetTherapistEditAsync(int id)
        {
            var therapist = await TherapistRepo.GetByIdAsync(id);
            return new TherapistEditViewModel {
                AppId = therapist.AppUserId,
                DBId = therapist.ID,
                Email=therapist.Email,
                _hourRate = therapist.hourRate,
                hourRate= therapist.hourRate,
                Phone = therapist.Phone,
                Specialization =therapist.Specialization,
                SeniorityLevel = therapist.SeniorityLevel
            };
        
        }

        public async Task<bool> SaveEditAsync(TherapistEditViewModel therapistModel)
        {
            if (therapistModel == null) return true;
            
            var therapist = await TherapistRepo.GetByIdAsync(therapistModel.DBId);
            
            therapist.PhotoURL = therapistModel.PhotoURL;
            therapist.hourRate = therapistModel.hourRate;
            therapist.Email = therapistModel.Email;
            therapist.appUser.Email = therapist.Email;
            therapist.Phone = therapistModel.Phone;
            therapist.Specialization = therapistModel.Specialization;
            therapist.SeniorityLevel = therapistModel.SeniorityLevel;
            string FilePath = string.Empty;
            if (therapistModel.Photo is not null)
            {
                var FolderPath = Path.Combine(webHostEnvironment.WebRootPath, "img", "Therapist", therapist.AppUserId.ToString());
                
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);
                
                string fileName = Guid.NewGuid().ToString() +Path.GetExtension(therapistModel.Photo.FileName);
                FilePath = Path.Combine(FolderPath, fileName);
                using (var fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    await therapistModel.Photo.CopyToAsync(fileStream);
                }
                therapist.PhotoURL = $"/img/Therapist/{therapist.AppUserId}/{fileName}";
            }



            if (therapistModel.changePasword)
            {
                var state = await _userManager.ChangePasswordAsync(therapist.appUser
                    , therapistModel.OldPassword
                    , therapistModel.NewPassword);

                if (state.Succeeded)
                {
                    await TherapistRepo.SaveAsync();
                    return false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        if (File.Exists(FilePath))
                        {
                            File.Delete(FilePath);
                        }
                    }
                    return true;
                }
            }
            else {
                await TherapistRepo.SaveAsync();    
                return false;
            }

        }

        public async Task<Therapist> AddNewTherapistAsync(TherapistCreateViewModel therapistModel)
        {


            var appuser = new ApplicationUser() {
                UserName = therapistModel.userName,
                Email = therapistModel.Email,
                type = AccountType.Therapist,
            };
            var Result = await _userManager.CreateAsync(appuser, therapistModel.Password);

            string PhotoURL = null;

            if (therapistModel.Photo is not null)
            {

                var FolderPath = Path.Combine(webHostEnvironment.WebRootPath,
                                            "img", "Therapist", appuser.Id);
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                string uniqueNamefile = Guid.NewGuid().ToString() +
                                           Path.GetExtension(therapistModel.Photo.FileName);
                string URL = Path.Combine(FolderPath, uniqueNamefile);
                using (var filestream = new FileStream(URL, FileMode.Create))
                {
                    await therapistModel.Photo.CopyToAsync(filestream);

                }
                PhotoURL = $"/img/Therapist/{appuser.Id}/{uniqueNamefile}";
            }
            await _userManager.AddToRoleAsync(appuser, ApplicationRole.Therapist);

            Therapist NewAddedTherapist = new Therapist()
            {
                Name = $"{therapistModel.FirstName} {therapistModel.LastName}",
                Email = therapistModel.Email,
                AppUserId = appuser.Id,
                Gender = therapistModel.Gender,
                hourRate = therapistModel.hourRate,
                Phone = therapistModel.Phone,
                Specialization = therapistModel.Specialization,
                SeniorityLevel = therapistModel.SeniorityLevel,
                RequirePasswordChange = true,
                PhotoURL = PhotoURL
            };

            if (Result.Succeeded)
            {
                await TherapistRepo.AddAsync(NewAddedTherapist);
                await TherapistRepo.SaveAsync();
                return NewAddedTherapist;
            }
            else
            {
                throw new Exception(Result.Errors.FirstOrDefault().Description.ToString());
            }

        }
    }
}
