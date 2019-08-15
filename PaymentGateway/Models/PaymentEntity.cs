using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models
{
    public class PaymentEntity : Payment
    {
        public Guid Id { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public PaymentEntityMasked ToPaymentEntityMasked()
        {
            return new PaymentEntityMasked()
            {
                Id = Id,
                PaymentStatus = PaymentStatus,
                CardNumber = CardNumber.ToString(),
                ExpiryYear = ExpiryYear.ToString(),
                ExpiryMonth = ExpiryMonth.ToString(),
                Amount = Amount,
                CurrencyCode = CurrencyCode,
                CVV = CVV.ToString()
            };
        }
    }
}
