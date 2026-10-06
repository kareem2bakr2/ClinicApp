namespace ClinicApp.Repository
{
    public class AppointmentRepository :
        GenericRepository<Appointment>,
        IAppointmentRepository
    {
        public AppointmentRepository(ClinicAppContext context) : base(context) { }

        public IQueryable<Appointment> GetAppointmentById(int id)
        {
            return _context.Appointments.Where(e=>e.ID == id);
        }

        public async Task<List<Appointment>> GetAppointmentByReceptionistAsync(int Receptid, DateTime start , DateTime end)
        {
            return await _context.Appointments.
                Where(a=>a.isDeleted == false 
                && a.ReceptID ==  Receptid
                ).Where(a=> ( a.StartDate > start && a.StartDate < end ) || a.StartDate == null )
                .ToListAsync();
        }
    }
}
