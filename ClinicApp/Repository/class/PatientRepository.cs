namespace ClinicApp.Repository
{
    public class PatientRepository : 
        GenericRepository<Patient>,
        IPatientRepository
    {
        public PatientRepository(ClinicAppContext clinicApp) : base(clinicApp)
        {

        }

        public async Task<Patient> PatientWithSessions(int id)
        {
            var patient =await  _context.Patients.AsNoTracking()
                        .Where(p=>p.ID == id && !p.isDeleted)
                        .Include(p=>p.appUser)
                        .Include(e => e.Appointments)
                        .ThenInclude(e=>e.MedicalService)
                        .Include(e=>e.Appointments)
                        .ThenInclude(e=>e.Therapist)
                        .Include(e => e.Appointments)
                        .ThenInclude(e => e.TherapySessions)
                        .FirstOrDefaultAsync();
            return patient;
        }



    }
}
