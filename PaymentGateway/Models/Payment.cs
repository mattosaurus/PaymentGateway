using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models
{
    public class Payment
    {
        public string CardNumber { get; set; }

        public int ExpiryYear { get; set; }

        public int ExpiryMonth { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public int CVV { get; set; }
    }
}
