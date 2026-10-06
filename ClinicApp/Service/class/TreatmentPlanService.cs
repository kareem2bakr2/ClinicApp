namespace ClinicApp.Service
{
    public class TreatmentPlanService : ITreatmentPlanService
    {
        private readonly ITreatmentPlanRepository treatmentPlanRepo;
        private readonly IExerciseRepository exerciseRepo;
        private readonly ITreatmentPlanExercisesRepository treatmentPlanExercisesRepo;
        public TreatmentPlanService(
            ITreatmentPlanRepository treatmentPlanRepo,
            IExerciseRepository exerciseRepo ,
            ITreatmentPlanExercisesRepository treatmentPlanExercisesRepo
            ) 
        {
            this.treatmentPlanExercisesRepo = treatmentPlanExercisesRepo;
            this.exerciseRepo = exerciseRepo;
            this.treatmentPlanRepo = treatmentPlanRepo;
        }

        public async Task <TreatmentPlan> AddNewExerciseAsync(int TreatPlanid, List<int> ExerciseIds)
        {                    
            var treatmentPlan = await treatmentPlanRepo.GetByIdAsync(TreatPlanid);
                            
            //var Exercise = await exerciseRepo.GetByIdAsync(ExerciseId);
            if (treatmentPlan is null ) return null;

            //treatmentPlan.TreatmentPlanExercises.Clear();

            foreach (var Exercise in ExerciseIds) {
                var ex = await exerciseRepo.GetByIdAsync(Exercise);
                if (ex is null) continue;
                var result = treatmentPlanExercisesRepo.Restore(TreatPlanid, Exercise);

                if (result.flag) continue;

                await treatmentPlanExercisesRepo
                    .AddAsync(
                    new TreatmentPlanExercises
                    {
                        Exercise = ex,
                        TreatmentPlan = treatmentPlan,
                    });
            }
            await treatmentPlanExercisesRepo.SaveAsync();
            return treatmentPlan;
        }

        public async Task<ReturnResult> RemoveExerciseAsync(int TreatPlanid, int ExerciseId)
        {
            var elem = treatmentPlanExercisesRepo
                .GetAll()
                .Where(e => e.treatmentPlanID == TreatPlanid && e.exerciseID == ExerciseId)
                .FirstOrDefault();
            
            await treatmentPlanExercisesRepo.Delete(elem);
            await treatmentPlanExercisesRepo.SaveAsync();

            return new ReturnResult { flag = true, message = "Saved Successfully" };
        }

        public async Task<TreatmentPlan> CreateTreatmentPlanAsync(TreatmentPlanCreateModelview model)
        {
            var newTratmentPlan = new TreatmentPlan
            {
                Notes = model.Notes,
                Name = model.Name,
                Category =model.Category,
                Level = model.Level,
            };
            await treatmentPlanRepo.AddAsync(newTratmentPlan);
            await treatmentPlanRepo.SaveAsync();
            return newTratmentPlan;
        }

        public async Task<ReturnResult> DeleteTreatmentPlanAsync(int id)
        {
            var tp = await treatmentPlanRepo.GetByIdAsync(id);
            if(tp is null )return new ReturnResult { flag = false , message ="Couldn't Complete Delete."};

            await treatmentPlanRepo.Delete(tp);
            await treatmentPlanRepo.SaveAsync();
            return new ReturnResult { flag = true, message = "Deleted Successfully" };
        }

        public async Task<TreatmentPlan> EditTreatmentPlanAsync(TreatmentPlanCreateModelview model)
        {
            var tpDB =await  treatmentPlanRepo.GetByIdAsync(model.ID);
            if (tpDB is null) return null;
            tpDB.Category = model.Category;
            tpDB.Level = model.Level;
            tpDB.Name = model.Name;
            tpDB.Notes = model.Notes;
            await treatmentPlanRepo.SaveAsync();
            return tpDB;
            
        }

        public async Task<TreatmentPlanIndexModelView> GetAllTreatmentPlansAsync(
            TreatmentPlanIndexModelView fitler) {
            return new TreatmentPlanIndexModelView {
                PageIndex = fitler.PageIndex,
                PageSize = fitler.PageSize,
                PauseCount = treatmentPlanRepo.GetAll().Count() ,
                _items = treatmentPlanRepo.GetAll()
                .Skip((fitler.PageIndex-1) * fitler.PageSize)
                .Take(fitler.PageSize)
                .Select(t => new TreatmentPlanModelview { 
                    Category = t.Category , 
                    ID = t.ID ,
                    Level = t.Level ,
                    Name = t.Name ,
                    Notes = t.Notes 
                }).ToList()
            };
        }

        public async Task<TreatmentPlanModelview> GetDetailsAsync(int id)
        {
            var tpdb = await  treatmentPlanRepo.GetByIdAsync(id);
            if (tpdb is null) return null;
            return new TreatmentPlanModelview
            {
                ID = tpdb.ID,
                Category = tpdb.Category,
                Level = tpdb.Level,
                Name = tpdb.Name,
                Notes = tpdb.Notes,
                Exercises = tpdb.TreatmentPlanExercises.Where(e => e.exerciseID is not null && 
                !e.isDeleted )
                .Select(t => new TreatmentPlanLinkModelview
                {
                    ID = t.exerciseID ?? 0,
                    Name = t.Exercise.Name
                }).ToList()
                ,
                AllExercises = exerciseRepo
                .GetAll()
                .Select(e => 
                new TreatmentPlanLinkModelview {
                    ID = e.ID,
                    Name = e.Name })
                .ToList()
            };

        }
    }   
}       
