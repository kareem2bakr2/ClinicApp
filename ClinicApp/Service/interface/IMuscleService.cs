namespace ClinicApp.Service
{
    public interface IMuscleService
    {
        Task<MuscleIndexViewModel> GetAllAsync(MuscleIndexViewModel filter);
        Task<MuscleViewModel> GetDetails(int id);

        Task<ReturnResult> EditMuscle(MuscleViewModel model);
        Task<ReturnResult> AddMuscle(MuscleViewModel model);

    }


}
