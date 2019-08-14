using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models
{
    public class PaymentEntityMasked : PaymentEntity
    {
        private string cardNumber;
        public new string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
            }
        }

        private string expiryYear;
        public new string ExpiryYear { get; set; }

        private string expiryMonth;
        public new string ExpiryMonth { get; set; }

        private string cvv;
        public new string CVV { get; set; }
    }
}
