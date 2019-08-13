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
        // In-memorary dictionary of payments, would be some kind of databasde in real life
        private Dictionary<Guid, PaymentEntity> payments = new Dictionary<Guid, PaymentEntity>();

        public PaymentResponse SetPayment(Payment payment)
        {
            //
            Guid paymentId = Guid.NewGuid();

            PaymentEntity paymentEntity = new PaymentEntity()
            {
                CardNumber = payment.CardNumber,
                ExpiryYear = payment.ExpiryYear,
                ExpiryMonth = payment.ExpiryMonth,
                Amount = payment.Amount,
                CurrencyCode = payment.CurrencyCode,
                CVV = payment.CVV
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
