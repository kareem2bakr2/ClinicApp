namespace ClinicApp.Service
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientListViewModel>> GetAllAsync();
        Task<TempCredentials> AddPatientasync(PatientCreateViewModel model);
        
        Task DeletePatietnAsync(int id);

        Task<PatientDetailsViewModel> GetPatientSessionsByIdAsync(int id);
        
        Task<PatientEditViewModel> GetPatientEditAsync(int id);
        Task<bool> SavePatientEditAsync(PatientEditViewModel patientViewModel);

    }
}
