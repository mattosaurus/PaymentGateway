using Microsoft.Extensions.Logging;
using PaymentGateway.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models.Clients
{
    public interface IPaymentGatewayClient
    {
        PaymentResponse SetPayment(Payment payment);

        PaymentEntity GetPayment(Guid id);
    }

    /// <summary>
    /// Client for performing payment gateway functions
    /// In real life we'd probably log details of the merchant request here
    /// as well as some logic to handle responses from the aqcuiring bank
    /// </summary>
    public class PaymentGatewayClient : IPaymentGatewayClient
    {
        private readonly IAcquiringBankService _acquiringBankService;
        private readonly ILogger _logger;

        public PaymentGatewayClient(IAcquiringBankService acquiringBankService, ILoggerFactory loggerFactory)
        {
            _acquiringBankService = acquiringBankService;
            _logger = loggerFactory.CreateLogger("PaymentGatewayClient");
        }

        public PaymentResponse SetPayment(Payment payment)
        {
            _logger.LogInformation("Sending payment to acquiring bank");
            PaymentResponse paymentResponse = _acquiringBankService.SetPayment(payment);

            // Assume that acquiring bank returns full payment details to us
            // However we want to return a masked output to the merchant

            return paymentResponse;
        }

        public PaymentEntity GetPayment(Guid id)
        {
            _logger.LogInformation("Retrieving payment details from acquiring bank");
            return _acquiringBankService.GetPayment(id);
        }
    }
}
