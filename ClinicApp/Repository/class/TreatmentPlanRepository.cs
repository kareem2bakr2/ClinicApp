namespace ClinicApp.Repository
{
    public class TreatmentPlanRepository : GenericRepository<TreatmentPlan>, ITreatmentPlanRepository
    {
        public TreatmentPlanRepository(ClinicAppContext clinicApp) : base(clinicApp)
        {
        }


    }
}
