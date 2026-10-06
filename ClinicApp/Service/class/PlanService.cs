using System.Reflection.Metadata.Ecma335;

namespace ClinicApp.Service
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository PlanRepo;
        public PlanService(IPlanRepository PlanRepo)
        {
            this.PlanRepo = PlanRepo;
        }

        public async Task<Plan> AddPlanAsync(PlanListModelView model)
        {
            var plan = new Plan
            {
                Name = model.Name,
                Price = model.Price,
                numberOfSession = model.numberOfSession,
            };
            await PlanRepo.AddAsync(plan);
            await PlanRepo.SaveAsync();
            return plan;
        }

        public async Task<PlanIndexModelView> GetAllPlansAsync(PlanIndexModelView filter) 
        {
            var plans = PlanRepo.GetAll();

            filter.PageCount =(int) Math.Ceiling((decimal)plans.Count() / filter.PageSize);
            filter._items = plans
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(s=>new PlanListModelView {
                    ID = s.ID,
                    Name = s.Name,
                    numberOfSession =s.numberOfSession,
                    Price = s.Price
                })
                .ToList();

            return filter;
        }

        public async Task<ReturnResult> DeletePlanAsync(int id)
        {
            var plan = await PlanRepo.GetByIdAsync(id);
            if(plan is null )
                return new ReturnResult { flag = false  , message = "Couldn't delete Plan"};
            await PlanRepo.Delete(plan);
            await PlanRepo.SaveAsync();
            return new ReturnResult { 
                flag = true,
                message = "Plan Deleted Sunccessfully" 
            };
        }

        public async Task<PlanDetailsModelView> GetPlanDetailsAsync(int id)
        {
            var plan = await PlanRepo.GetByIdAsync(id);
            if (plan is null) return null;

            return new PlanDetailsModelView 
            {
                ID= plan.ID,
                Name= plan.Name,
                numberOfSession= plan.numberOfSession, 
                Price= plan.Price,
                _items = plan.Appointments.Select(a=>
                new PlanAppointmentDetailsModelView 
                {ID = a.ID,
                EndDate= a.EndDate,
                StartDate =a.StartDate,
                Status = a.Status
                }).ToList()
            };
        }
        public async Task<ReturnResult> EditPlanAsync(PlanDetailsModelView model)
        {
            var plan = await PlanRepo.GetByIdAsync(model.ID);
            if (plan is null) return new ReturnResult {flag = false , message ="Failed to edit Plan" };

            plan.Price = model.Price;
            plan.numberOfSession = model.numberOfSession;
            plan.Name = model.Name;
            
            await PlanRepo.SaveAsync();
            return new ReturnResult { flag = true, message = "Saved Successfully" };
            
        }
    }
}
