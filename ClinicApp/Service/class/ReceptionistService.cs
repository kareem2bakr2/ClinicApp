using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Service
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IReceptionistRepository receptionistRepo;
        private readonly IWebHostEnvironment _webHost;
        private readonly IAppointmentRepository appointmentRepo;
        private readonly IPaymentRepository paymentRepository;
        private readonly ICurrentUserService currentUserService;

        public ReceptionistService(
                    IReceptionistRepository receptionistRepo,
                    IAppointmentRepository appointmentRepo,
                    IPaymentRepository paymentRepository,
                    UserManager<ApplicationUser> userManager,
                    IWebHostEnvironment _webHost,
                    ICurrentUserService currentUserService
        ) { 
           
            this.currentUserService = currentUserService;
            this.receptionistRepo = receptionistRepo;
            this.userManager = userManager;
            this._webHost = _webHost;
            this.paymentRepository = paymentRepository;
            this.appointmentRepo = appointmentRepo;
        }

        public async Task<Receptionist> CreateNewReceptionistAsync(ReceptionistCreateViewModel ReceptModel)
        {
            var appuser = new ApplicationUser() {
                UserName = ReceptModel.UserName,
                type = AccountType.Receptionist,
            };

            var result =await  userManager.CreateAsync( appuser, ReceptModel.Password );
            if (result.Succeeded) {

                string URL = null;
                if (ReceptModel.Photo is not null)
                {


                    string folderPath = Path.Combine(_webHost.WebRootPath,"img", "Receptionist" ,appuser.Id);
                    //if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                    //folderPath = Path.Combine(folderPath, appuser.Id);

                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                    string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(ReceptModel.Photo.FileName);
                    var filePath = Path.Combine(folderPath, uniqueName);
                    using (var filestream = new FileStream(filePath,FileMode.Create)) 
                    {
                        await ReceptModel.Photo.CopyToAsync(filestream);
                    }
                    URL = $"/img/Receptionist/{appuser.Id}/{uniqueName}";

                }
                var receptionist = new Receptionist()
                {
                    Name = $"{ReceptModel.FirstName} {ReceptModel.LastName}",
                    Age = (int)(DateTime.Today - ReceptModel.BOD.ToDateTime(TimeOnly.MinValue)).TotalDays / 365,
                    AppUserId =appuser.Id,
                    Dayoff  = ReceptModel.Dayoff,
                    Phone = ReceptModel.Phone,
                    PhotoURL=URL,
                    WorktimeFrom = ReceptModel.WorktimeFrom,
                    WorkTimeTO = ReceptModel.WorkTimeTO,
                    Gender = ReceptModel.Gender
                };
                await userManager.AddToRoleAsync(appuser, ApplicationRole.Receptionist);

                await receptionistRepo.AddAsync(receptionist);
                await receptionistRepo.SaveAsync();
                return receptionist;
            }
            else {
                throw new Exception(result.Errors.FirstOrDefault().Description);
                
            }

        }


        public async Task<bool> DeleteAsync(int id)
        {
            var receptionist = await  receptionistRepo.GetByIdAsync(id);
            if (receptionist is null) return false;
            receptionist.isDeleted = true;
            await receptionistRepo.SaveAsync();
            return true;
        }

        public async Task<ReceptionistDetailsViewModel> GetReceptionistDetailsAsync(int id)
        {
            if (!await IsReceptAuth(id)) return null;


            var Receptionist =await receptionistRepo.GetByIdAsync(id);
            var appointments = await appointmentRepo.GetAppointmentByReceptionistAsync(id,
                                                    DateTime.Today.AddMonths(-4),
                                                    DateTime.MaxValue);
            var payments = await paymentRepository
                            .PaymentByReseptionistIdAsync(id ,
                            DateTime.Today.AddMonths(-5),
                            DateTime.MaxValue);
            if (Receptionist is null) return null;

            var paymentsModel 
                = new HashSet<ReceptionistPymentDetailsViewModel>();
            var appintmentsModel 
                = new HashSet<ReceptionistAppointmentsDetailsViewModel>();

            foreach (var appointment in appointments) {

                appintmentsModel.Add(
                        new ReceptionistAppointmentsDetailsViewModel
                        {
                            Discount = appointment.Discount,
                            DiscountPercentage = appointment.DiscountPercentage,
                            EndDate= appointment.EndDate,
                            FinalPrice = appointment.FinalPrice,
                            ID= appointment.ID,
                            medicalServiceName =appointment.MedicalService?.Name,
                            Notes = appointment.Notes,
                            PatientName =appointment.Patient?.Name,
                            StartDate = appointment.StartDate,
                            Status = appointment.Status
                        });
            }

            foreach(var payment in payments)
            {
                paymentsModel.Add(new ReceptionistPymentDetailsViewModel { 
                    Date = payment.Date,
                    ID = payment.ID,
                    IsCredit = payment.IsCredit,
                    Method = payment.Method,
                    Notes = payment.Notes
                });

            }
            return new ReceptionistDetailsViewModel() {
                Payments = paymentsModel,
                Appointments = appintmentsModel,
                ID = Receptionist.ID,
                Age = Receptionist.Age,
                Dayoff = Receptionist.Dayoff,
                Gender = Receptionist.Gender,
                Name = Receptionist.Name,
                Phone = Receptionist.Phone,
                PhotoURL = Receptionist.PhotoURL,
                WorktimeFrom = Receptionist.WorktimeFrom,
                WorkTimeTO = Receptionist.WorkTimeTO
            };
        }

        public async  Task<List<ReceptionistListViewModel>> GetReceptionistListViewModelAsync()
        {
            var recepts = receptionistRepo.GetAll().OrderByDescending(e=>e.CreationDate).ToList();
            var receptsModel = new List<ReceptionistListViewModel>();
            foreach (var receptionist in recepts) {
                receptsModel.Add(new ReceptionistListViewModel 
                {
                    ID = receptionist.ID,
                    Dayoff= receptionist.Dayoff,
                    Name = receptionist.Name,
                    Phone = receptionist.Phone,
                    PhotoURL = receptionist.PhotoURL,
                    WorktimeFrom = receptionist.WorktimeFrom,
                    WorkTimeTO =  receptionist.WorkTimeTO
                });
            }
            return receptsModel;
        }

        public async Task<ReceptionistEditViewModel> GetReceptionistEditAsync(int id) { 

            var receptioninst = await receptionistRepo.GetByIdAsync(id);
            if(!await IsReceptAuth(id))return null;

            return new ReceptionistEditViewModel() {
                ID = id,    
                Dayoff= receptioninst.Dayoff,
                Phone = receptioninst.Phone,
                PhotoURL = receptioninst.PhotoURL,
                WorktimeFrom = receptioninst.WorktimeFrom,
                WorkTimeTO = receptioninst.WorkTimeTO
            };
            
        }
        public async Task<bool> SaveReceptionistEditAsync(ReceptionistEditViewModel model)
        {

            if (model.ID == 0) return false;

            if (!await IsReceptAuth(model.ID)) return false;

            var receptionistDB = await receptionistRepo.GetByIdAsync(model.ID);
            
            if (model == null) return false;
            if (model.changePasword)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(receptionistDB.appUser);
                
                var result = await userManager.ResetPasswordAsync(receptionistDB.appUser, token , model.NewPassword);
                if (!result.Succeeded) return false;

            }
            if (model.Photo is not null) {
                var FolderPath = Path.Combine( _webHost.WebRootPath ,"img", "Receptionist",receptionistDB.AppUserId);
                
                if(!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            
                var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                
                var filePath = Path.Combine(FolderPath, uniqueName);

                using (var filestream = new FileStream(filePath, FileMode.Create)) 
                {
                    await model.Photo.CopyToAsync(filestream);
                }
                var URL = $"/img/Receptionist/{receptionistDB.AppUserId}/{uniqueName}";
                model.PhotoURL = URL;
            }
            receptionistDB.PhotoURL = model.PhotoURL;
            receptionistDB.Dayoff = model.Dayoff;
            receptionistDB.WorktimeFrom = model.WorktimeFrom;
            receptionistDB.WorkTimeTO = model.WorkTimeTO;
            receptionistDB.Phone = model.Phone;
            await receptionistRepo.SaveAsync();
            return true;
        }

        private async Task<bool> IsReceptAuth(int id)
        {
            var userId = currentUserService.CurrentUserID;
            var user = await userManager.FindByIdAsync(userId);
            bool isPatientRole = await userManager.IsInRoleAsync(user, ApplicationRole.Patient);

            if (user is null) return false;
            if (isPatientRole && (user.Patient is null || user.Patient.ID != id)) return false;
            return true;

        }
    
    
    }
}
