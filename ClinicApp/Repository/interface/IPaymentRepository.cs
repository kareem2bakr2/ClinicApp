namespace ClinicApp.Repository
{
    public interface  IPaymentRepository : IGenericRepository<Payment>
    {
        Task<List<Payment>> PaymentByTherapistIdAsync (int TherapistId);
        Task<List<Payment>> PaymentByReseptionistIdAsync (int ReceptId, DateTime Start, DateTime end);

    }
}
