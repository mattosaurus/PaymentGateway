using PaymentGateway.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models
{
    public class PaymentEntityMasked : PaymentEntity
    {
        public new string CardNumber { get; set; }

        public new string ExpiryYear { get; set; }

        public new string ExpiryMonth { get; set; }

        public new string CVV { get; set; }
    }
}
