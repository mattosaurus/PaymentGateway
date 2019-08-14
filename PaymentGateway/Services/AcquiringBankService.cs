using PaymentGateway.Models;
using PaymentGateway.Models.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services
{
    public interface IAcquiringBankService
    {
        PaymentResponse SetPayment(Payment payment);

        PaymentEntity GetPayment(Guid id);
    }

    public class AcquiringBankService : IAcquiringBankService
    {
        private readonly IAcquiringBankClient _acquiringBankClient;

        public AcquiringBankService(IAcquiringBankClient acquiringBankClient)
        {
            _acquiringBankClient = acquiringBankClient;
        }

        public PaymentResponse SetPayment(Payment payment)
        {
            return _acquiringBankClient.SetPayment(payment);
        }

        public PaymentEntity GetPayment(Guid id)
        {
            return _acquiringBankClient.GetPayment(id);
        }
    }
}
