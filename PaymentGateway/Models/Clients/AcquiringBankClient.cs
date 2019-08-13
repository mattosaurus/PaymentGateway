using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models.Clients
{
    public interface IAcquiringBankClient
    {
        PaymentResponse SetPayment(Payment payment);
    }

    public class AcquiringBankClient : IAcquiringBankClient
    {
        private Dictionary<Guid, PaymentEntity> payments = new Dictionary<Guid, PaymentEntity>();

        PaymentResponse SetPayment(Payment payment)
        {
            Guid paymentId = Guid.NewGuid();

            PaymentEntity paymentEntity = new PaymentEntity()
            {
                Id = paymentId,

            };

            payments.Add(paymentId, paymentEntity);
            PaymentResponse paymentResponse = new PaymentResponse()
            {
                Id = paymentId,
                PaymentStatus = PaymentStatus.Success
            };

            return paymentResponse;
        }
    }
}
