namespace ClinicApp.Service
{
    public interface ITherapistService
    {
        Task<List<TherapistListViewModel>> GetMyStaffTherapistsAsync(int? managerid);
        Task<List<TherapistAddToMyTeamModelView>> GetTherapistWithNoMGR(int id);

        Task ChangeTherapistTeam(int [] TherapistsIdToTransfer , string managerAppID);
        Task removeFromTeamAsync(int id, int managerid);

        Task <TherapistDetialsViewModel> GetTherapistDetialsAsync(int id);
        Task<TherapistEditViewModel> GetTherapistEditAsync(int id);

        Task<bool> SaveEditAsync(TherapistEditViewModel therapistModel);
        Task<Therapist> AddNewTherapistAsync(TherapistCreateViewModel therapistModel);

    }
}
