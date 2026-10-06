namespace ClinicApp.Service
{
    public interface IMedicalServiceService
    {
        Task<MediaclServiceIndexViewModel> GetAllMedicalService(MediaclServiceIndexViewModel model);

        Task<MedicalService> CreateMedicalServiceAsync(MedicalServiceViewModel model);

        Task<MedicalServiceDetailsViewModel> GetMedicalServiceDetailsasync(MedicalServiceDetailsViewModel model);

        Task<ReturnResult> EditMedicalServiceAsync(MedicalServiceViewModel model); 

    }
}
