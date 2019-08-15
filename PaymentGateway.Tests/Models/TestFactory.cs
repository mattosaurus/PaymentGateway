using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PaymentGateway.Tests.Models
{
    public class TestFactory
    {
        public static Payment GetTestPayment()
        {
            Payment payment = new Payment()
            {
                CardNumber = "1234567891234567",
                ExpiryYear = 2019,
                ExpiryMonth = 8,
                Amount = (decimal)19.99,
                CurrencyCode = "GBP",
                CVV = 123
            };

            return payment;
        }

        public static PaymentResponse GetTestPaymentResponse()
        {
            PaymentResponse paymentResponse = new PaymentResponse()
            {
                Id = new Guid(),
                PaymentStatus = PaymentStatus.Success
            };

            return paymentResponse;
        }

        public static PaymentEntity GetTestPaymentEntityResponse(string id)
        {
            Payment payment = GetTestPayment();

            PaymentEntity paymentResponse = new PaymentEntity()
            {
                Id = Guid.Parse(id),
                PaymentStatus = PaymentStatus.Success,
                CardNumber = payment.CardNumber,
                ExpiryYear = payment.ExpiryYear,
                ExpiryMonth = payment.ExpiryMonth,
                Amount = payment.Amount,
                CurrencyCode = payment.CurrencyCode,
                CVV = payment.CVV
            };

            return paymentResponse;
        }

        private static Dictionary<string, StringValues> CreateDictionary(string key, string value)
        {
            var qs = new Dictionary<string, StringValues>
            {
                { key, value }
            };
            return qs;
        }

        public static DefaultHttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(CreateDictionary(queryStringKey, queryStringValue))
            };
            return request;
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            return request;
        }

        public static DefaultHttpRequest CreateHttpRequest(object body)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);

            var json = JsonConvert.SerializeObject(body);

            streamWriter.Write(json);
            streamWriter.Flush();

            memoryStream.Position = 0;

            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.Body = memoryStream;

            return request;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
