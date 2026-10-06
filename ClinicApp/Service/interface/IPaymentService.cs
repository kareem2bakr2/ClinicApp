namespace ClinicApp.Service
{
    public interface IPaymentService
    {
        Task<PaymentIndexModelView> GetAllAsync(PaymentIndexModelView filter);

    }
}
