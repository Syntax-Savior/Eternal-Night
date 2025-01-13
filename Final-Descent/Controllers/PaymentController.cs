using System.Threading.Tasks;
using Final_Descent.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final_Descent.Controllers
{
    [Route("Payment")]
    public class PaymentController : Controller
    {
        private readonly ChapaService _chapaService;

        public PaymentController(ChapaService chapaService)
        {
            _chapaService = chapaService;
        }

        [HttpPost("Initialize")]
        public async Task<IActionResult> Initialize(string amount, string phoneNumber)
        {
            try
            {
                var response = await _chapaService.InitializeTransactionAsync(amount, phoneNumber);

                var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);

                if (responseObject.status == "success")
                {
                    return Redirect(responseObject.data.checkout_url.ToString());
                }

                TempData["ErrorMessage"] = "Failed to initialize payment. Please try again.";
                return RedirectToAction("Index", "Product");
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while initializing the payment.";
                return RedirectToAction("Index", "Product");
            }
        }
    }
}
