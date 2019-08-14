using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PaymentGateway.Models.Clients
{
    public interface IAcquiringBankClient
    {
        PaymentResponse SetPayment(Payment payment);

        PaymentEntity GetPayment(Guid id);
    }

    /// <summary>
    /// Client for interacting with the acquiring bank, this would probably be via API in real life
    /// </summary>
    public class AcquiringBankClient : IAcquiringBankClient
    {
        // In-memory dictionary of payments, in real life wouldn't be needed as these would be retrieved from the acquiring bank
        private ConcurrentDictionary<Guid, PaymentEntity> payments = new ConcurrentDictionary<Guid, PaymentEntity>();

        public PaymentResponse SetPayment(Payment payment)
        {
            // Generate id for the payment
            Guid paymentId = Guid.NewGuid();

            // 5% chance of payment failure to make it more interesting
            PaymentStatus paymentStatus = PaymentStatus.Success;

            if (GetRandomInteger(0, 100) < 5)
            {
                paymentStatus = PaymentStatus.Failure;
            }

            PaymentEntity paymentEntity = new PaymentEntity()
            {
                Id = paymentId,
                PaymentStatus = paymentStatus,
                CardNumber = payment.CardNumber,
                ExpiryYear = payment.ExpiryYear,
                ExpiryMonth = payment.ExpiryMonth,
                Amount = payment.Amount,
                CurrencyCode = payment.CurrencyCode,
                CVV = payment.CVV
            };

            payments.TryAdd(paymentId, paymentEntity);

            PaymentResponse paymentResponse = new PaymentResponse()
            {
                Id = paymentEntity.Id,
                PaymentStatus = paymentEntity.PaymentStatus
            };

            return paymentResponse;
        }

        public PaymentEntity GetPayment(Guid id)
        {
            return payments[id];
        }

        private int GetRandomInteger(int minValue = 0, int maxValue = int.MaxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("Maximum value must be greater than minimum value");
            }
            else if (maxValue == minValue)
            {
                return 0;
            }

            Int64 diff = maxValue - minValue;

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                while (true)
                {
                    byte[] fourBytes = new byte[4];
                    crypto.GetBytes(fourBytes);

                    // Convert that into an uint.
                    UInt32 scale = BitConverter.ToUInt32(fourBytes, 0);

                    Int64 max = (1 + (Int64)UInt32.MaxValue);
                    Int64 remainder = max % diff;
                    if (scale < max - remainder)
                    {
                        return (Int32)(minValue + (scale % diff));
                    }
                }
            }
        }
    }
}
