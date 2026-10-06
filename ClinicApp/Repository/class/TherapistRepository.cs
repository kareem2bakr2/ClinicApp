namespace ClinicApp.Repository
{
    public class TherapistRepository :
        GenericRepository<Therapist>,
        ITherapistRepository
    {

        public TherapistRepository(ClinicAppContext clinicAppContext) : base(clinicAppContext) 
        {
            
        }

        public async Task<IEnumerable<Therapist>> GetMyStaffTherapistsAsync(int managerid)
        {
            return await _context.Therapists.Where(e=>e.ManagerID == managerid && !e.isDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Therapist>> GetTherapistWithNoMGR() 
        { 
            return await _context.Therapists
                .Where(t=> t.ManagerID == null && 
                       t.ManagerID != (int)StaticNums.AppManagerID &&
                       
                       !t.isDeleted).ToListAsync();
        }
        public async Task<Therapist> GetTherapistFromAppIdAsync(string AppId) 
        {
            return await  _context.Therapists
                            .AsNoTracking()
                            .FirstOrDefaultAsync(e => e.AppUserId == AppId);
        }

        public async Task<Therapist> GetTherapistWithAppointmentsSessionsPaymentAsync(int Therapistid)
        {
            return await _context.Therapists
                                 .Where(t => t.ID == Therapistid)
                                 .AsNoTracking()
                                 .Include(e=>e.Manager)
                                 .Include(t=>t.TherapySessions)
                                 .ThenInclude(t=>t.TreatmentPlan)
                                 .Include(t=>t.Payments)
                                 .Include(t => t.Appointments)
                                 .ThenInclude(t=>t.Plan)
                                 .Include(t=>t.Appointments)
                                 .ThenInclude(t=>t.Patient)
                                 .FirstOrDefaultAsync();
        }

        //public async Task<Therapist> GetTherapistWithAppointmentsAsync(int Therapistid)
        //{

        //}
    }

}
