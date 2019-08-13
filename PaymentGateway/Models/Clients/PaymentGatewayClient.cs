using PaymentGateway.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models.Clients
{
    public interface IPaymentGatewayClient
    {
        PaymentResponse SetPayment(Payment payment);
    }

    public class PaymentGatewayClient : IPaymentGatewayClient
    {
        private readonly AcquiringBankService _acquiringBankService;

        public PaymentGatewayClient(AcquiringBankService acquiringBankService)
        {
            _acquiringBankService = acquiringBankService;
        }

        public SetPayment(Payment payment)
        {
            _acquiringBankService.SetPayment(payment);
        }
    }
}
