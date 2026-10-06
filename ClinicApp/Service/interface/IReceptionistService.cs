namespace ClinicApp.Service
{
    public interface IReceptionistService
    {
        Task<List<ReceptionistListViewModel>> GetReceptionistListViewModelAsync();
        Task<bool> DeleteAsync(int id);
        Task<Receptionist> CreateNewReceptionistAsync(ReceptionistCreateViewModel ReceptModel);
        Task<ReceptionistDetailsViewModel> GetReceptionistDetailsAsync(int id);
        Task<ReceptionistEditViewModel> GetReceptionistEditAsync(int id);
        Task<bool> SaveReceptionistEditAsync(ReceptionistEditViewModel model);


    }
}