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

            string name = id;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string");
        }

        [FunctionName("SetPayment")]
        public async Task<IActionResult> SetPayment([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Payment")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a POST request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string name = data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name in the request body");
        }
    }
}
