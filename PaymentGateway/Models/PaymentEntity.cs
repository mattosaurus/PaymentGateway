using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Models
{
    public class PaymentEntity : Payment
    {
        public Guid Id { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
