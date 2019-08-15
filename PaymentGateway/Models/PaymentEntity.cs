using PaymentGateway.Extensions;
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
                CardNumber = CardNumber.Mask(0, 12),
                ExpiryYear = ExpiryYear.ToStringMask(0, 4),
                ExpiryMonth = ExpiryMonth.ToString("D2").Mask(0, 2),
                Amount = Amount,
                CurrencyCode = CurrencyCode,
                CVV = CVV.ToStringMask(0, 3)
            };
        }
    }
}
