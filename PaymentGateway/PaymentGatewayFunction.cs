using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway;
using Microsoft.Azure.WebJobs.Hosting;
using PaymentGateway.Services;
using PaymentGateway.Models;

[assembly: WebJobsStartup(typeof(Startup))]

namespace PaymentGateway
{
    public class PaymentGatewayFunction
    {
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly ILogger _logger;

        public PaymentGatewayFunction(IPaymentGatewayService paymentGatewayService, ILoggerFactory loggerFactory)
        {
            _paymentGatewayService = paymentGatewayService;
            _logger = loggerFactory.CreateLogger("PaymentGatewayFunction");
        }

        [FunctionName("GetPayment")]
        public async Task<IActionResult> GetPayment([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Payment/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a GET request.");

            Guid paymentId; 
            if (id == null || !Guid.TryParse(id, out paymentId))
            {
                return new BadRequestObjectResult("No ID parameter provided");
            }

            PaymentEntity paymentEntity = _paymentGatewayService.GetPayment(paymentId);

            return new OkObjectResult(paymentEntity.ToPaymentEntityMasked());
        }

        [FunctionName("SetPayment")]
        public async Task<IActionResult> SetPayment([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Payment")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a POST request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Payment payment = JsonConvert.DeserializeObject<Payment>(requestBody);

            PaymentResponse paymentResponse = _paymentGatewayService.SetPayment(payment);

            return new OkObjectResult(paymentResponse);
        }
    }
}
