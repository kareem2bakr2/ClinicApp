namespace ClinicApp.Repository
{
    public class PaymentRepository:
        GenericRepository<Payment>,
        IPaymentRepository
    {
        public PaymentRepository(ClinicAppContext context) : base(context) 
        { 
            
        }

        public async  Task<List<Payment>> PaymentByReseptionistIdAsync(int ReceptId ,DateTime Start, DateTime end )
        {
            return await _context
                        .Payments
                        .Where(p => !p.isDeleted && p.ReceptID == ReceptId)
                        .Where(p=> p.Date > Start && p.Date < end )
                        .ToListAsync();

        }

        public async Task<List<Payment>> PaymentByTherapistIdAsync(int TherapistId)
        {
            return await _context
                        .Payments
                        .Where(e => !e.isDeleted  && e.TherapistID == TherapistId)
                        .ToListAsync();
        }

    }
}
