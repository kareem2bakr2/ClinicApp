using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    [Authorize(Roles = ApplicationRole.Admin)]
    public class PaymentController : Controller
    {
        private readonly IPaymentService paymentService;
        public PaymentController(IPaymentService paymentService) {
            this.paymentService = paymentService;
        }
        public async Task<IActionResult> Index(PaymentIndexModelView filter)
        {
            var model =await  paymentService.GetAllAsync(filter);
            return View(model);
        }
    }
}
