namespace ClinicApp.Service
{
    public interface IPlanService
    {
        Task<PlanIndexModelView> GetAllPlansAsync(PlanIndexModelView filter);
        Task<Plan> AddPlanAsync(PlanListModelView model);
        Task<ReturnResult> DeletePlanAsync(int id);
        Task<PlanDetailsModelView> GetPlanDetailsAsync(int id);
        Task<ReturnResult> EditPlanAsync(PlanDetailsModelView model);

    }
}
