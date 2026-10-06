namespace ClinicApp.Repository
{
    public class PlanRepository :
        GenericRepository<Plan>,
        IPlanRepository
    {
        public PlanRepository(ClinicAppContext clinicapp) : base(clinicapp) 
        {
        
        }

    }
}
