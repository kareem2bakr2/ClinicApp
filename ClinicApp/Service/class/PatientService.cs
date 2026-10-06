

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ClinicApp.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository PatientRepo;
        private readonly ITherapySessionRepository TherapySessionRepo;        
        private readonly IAppointmentRepository AppointmentRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly
             ICurrentUserService _currentUserService; 

        public PatientService(IPatientRepository PatientRepo ,
            ITherapySessionRepository TherapySessionRepo,
            IAppointmentRepository AppointmentRepo,
            UserManager<ApplicationUser> _userManager ,
             ICurrentUserService currentUserService
            ) 
        {
            this.PatientRepo = PatientRepo;
            this.TherapySessionRepo = TherapySessionRepo;
            this.AppointmentRepo = AppointmentRepo;
            this._userManager = _userManager;
            this._currentUserService = currentUserService;
        }

        public async Task<TempCredentials> AddPatientasync(PatientCreateViewModel model)
        {
            string username="";
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                string[] nameParts = model.Name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string firstName = nameParts[0];
                string lastName = nameParts[nameParts.Length - 1];
                username = $"{firstName}{lastName}".ToLower();
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model?.Email,
                UserName = username,
                PhoneNumber = model.Phone,
                type = AccountType.Patient
            };

            await _userManager.CreateAsync(user , model.Password);
            await _userManager.AddToRoleAsync(user, ApplicationRole.Patient);

            await PatientRepo.AddAsync(new Patient()
            {
                Address = model.Address,
                Email = model.Email,
                RegistrationDate = DateTime.Now,
                Phone = model.Phone,
                Gender = model.Gender,
                Name = model.Name,
                EmergencyRelationShip = model.EmergencyRelationShip,
                EmergencyPhone = model.EmergencyPhone,
                EmergencyContact = model.EmergencyContact,
                DOB = model.DOB,
                RequirePasswordChange= model.RequirePasswordChange,
                appUser = user
            });
            await PatientRepo.SaveAsync();
            return new TempCredentials() { userName = username, password = model.Password };
        }

        public async Task DeletePatietnAsync(int id)
        {
            var patient = await PatientRepo.GetByIdAsync(id);
            await PatientRepo.Delete(patient);
            await PatientRepo.SaveAsync();
             
        }

        public async Task<IEnumerable<PatientListViewModel>> GetAllAsync()
        {
            var patients = PatientRepo.GetAll();
            var patientsViewModel = patients.Select(p => new PatientListViewModel
            {
                Id = p.ID,
                Name = p.Name,
                Age = (int)((DateTime.Now - p.DOB).TotalDays / 365),
                Email = p.Email,
                Phone = p.Phone,
                EmergencyContact = p.EmergencyContact,
                EmergencyPhone = p.EmergencyPhone,
                session = p.Appointments
                                    .SelectMany(a => a.TherapySessions)
                                    .Where(a => a.SessionDate >= DateOnly.FromDateTime(DateTime.Today))
                                    .OrderBy(e => e.SessionDate)
                                    .FirstOrDefault(),
            }).ToList().OrderBy(e=>e.session is null ? 1:0)
            .ThenBy(e=>e.session?.SessionDate);
            return patientsViewModel;
        }

        public async Task<PatientEditViewModel> GetPatientEditAsync(int id)
        {
            bool auth = await isPatientAuth(id);

            if (!auth) return null;

            var patient = await PatientRepo.GetByIdAsync(id);
            var patientEditModel = new PatientEditViewModel() 
            { 
                ID = patient.ID,
                Email = patient.Email,
                Address = patient.Address,
                Phone = patient.Phone,
                EmergencyContact= patient.EmergencyContact,
                EmergencyPhone= patient.EmergencyPhone,
                EmergencyRelationShip = patient.EmergencyRelationShip,    
            };
            // when saving edit don't forget to update app  id with email address;

            return patientEditModel;    
        }

        public async Task<bool> SavePatientEditAsync(PatientEditViewModel patientViewModel)
        {
            bool auth = await isPatientAuth(patientViewModel.ID);

            if (!auth) return false;

            var patientDB = await PatientRepo.GetByIdAsync(patientViewModel.ID);

            patientDB.Email = patientViewModel.Email;
            patientDB.appUser.Email = patientViewModel.Email;

            patientDB.EmergencyPhone = patientViewModel.EmergencyPhone;
            patientDB.EmergencyContact = patientViewModel.EmergencyContact;
            patientDB.EmergencyRelationShip = patientViewModel.EmergencyRelationShip;
            patientDB.Phone = patientViewModel.Phone;
            patientDB.Address = patientViewModel.Address;

            await PatientRepo.SaveAsync();

            if (patientViewModel.changePassword)
            {
                var result = await _userManager.ChangePasswordAsync(patientDB.appUser,
                                                patientViewModel.oldPassword,
                                                patientViewModel.NewPassword);
                return result.Succeeded;
            }
            return true;
        }
        public async Task<PatientDetailsViewModel> GetPatientSessionsByIdAsync(int id)
        {
            bool auth = await isPatientAuth(id);
           
            if(!auth) return null;

            var patient = await PatientRepo.PatientWithSessions(id);
            if (patient is null) return null;
            var PatientAppointment = patient.Appointments.OrderByDescending(a => a.StartDate);

            var appAppointments 
                = new HashSet<PatientAppointmentDetailsViewModel>();

            foreach (var appointment in PatientAppointment) 
            {
                appAppointments.Add(new PatientAppointmentDetailsViewModel() {
                    ID = appointment.ID,
                    StartDate= DateOnly.FromDateTime(appointment.StartDate?? DateTime.MaxValue),
                    EndDate = DateOnly.FromDateTime(appointment.EndDate?? DateTime.MaxValue) ,
                    Status = appointment.Status,
                    medicalServiceName = appointment.MedicalService?.Name,
                    TherapistName = appointment.Therapist?.Name
                });
            }
            var patientModel = new PatientDetailsViewModel()
            {
                ID = patient.ID,
                Email = patient.Email,
                RegistrationDate = patient.RegistrationDate,
                Address = patient.Address,
                Phone = patient.Phone,
                Name = patient.Name,
                Gender = patient.Gender,
                EmergencyContact = patient.EmergencyContact,
                EmergencyPhone = patient.EmergencyPhone,
                EmergencyRelationShip = patient.EmergencyRelationShip,
                DOB = DateOnly.FromDateTime(patient.DOB),
                Age = (int)((DateTime.Now - patient.DOB).TotalDays / 365),
                UserName = patient.appUser.UserName,
                Appointments = appAppointments
            };

            return patientModel;
        
        }
    
        private async Task<bool> isPatientAuth(int id)
        {
            var userId = _currentUserService.CurrentUserID;
            var user = await _userManager.FindByIdAsync(userId);
            bool isPatient = await _userManager.IsInRoleAsync(user, ApplicationRole.Patient);

            if (isPatient)
            {
                var patientDBId = user.Patient?.ID;
                if (patientDBId is null ||
                    patientDBId != id)
                {
                    return false;
                }

            }
            return true;

        }

    }
}
