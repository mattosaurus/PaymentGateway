using PaymentGateway.Extensions;
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
                return cardNumber.Mask(0, 12);
            }
            set
            {
                cardNumber = value;
            }
        }

        private string expiryYear;
        public new string ExpiryYear
        {
            get
            {
                return expiryYear.Mask(0, 4);
            }
            set
            {
                expiryYear = value;
            }
        }

        private string expiryMonth;
        public new string ExpiryMonth
        {
            get
            {
                return expiryMonth.Mask(0, 2);
            }
            set
            {
                expiryMonth = value;
            }
        }

        private string cvv;
        public new string CVV
        {
            get
            {
                return cvv.Mask(0, 3);
            }
            set
            {
                cvv = value;
            }
        }
    }
}
