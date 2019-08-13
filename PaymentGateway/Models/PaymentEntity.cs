using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models
{
    public class PaymentEntity : Payment
    {
        public PaymentStatus PaymentStatus { get; set; }
    }
}
