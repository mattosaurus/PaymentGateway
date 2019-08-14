using PaymentGateway.Models;
using PaymentGateway.Models.Clients;
using System;

namespace PaymentGateway.Services
{
    public interface IPaymentGatewayService
    {
        PaymentResponse SetPayment(Payment payment);

        PaymentEntity GetPayment(Guid id);
    }

    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly IPaymentGatewayClient _paymentGatewayClient;

        public PaymentGatewayService(IPaymentGatewayClient paymentGatewayClient)
        {
            _paymentGatewayClient = paymentGatewayClient;
        }

        public PaymentResponse SetPayment(Payment payment)
        {
            return _paymentGatewayClient.SetPayment(payment);
        }

        public PaymentEntity GetPayment(Guid id)
        {
            return _paymentGatewayClient.GetPayment(id);
        }
    }
}
