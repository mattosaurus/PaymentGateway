using PaymentGateway.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace PaymentGateway.LoadTest
{
    class Program
    {
        private static string baseUrl = "https://checkoutpaymentgateway.azurewebsites.net/api/Payment/";
        private static string code = "dhiT/S3SIEIqP2W9aHaYAJ6c1cIN32ika6uqpXYTrnQX8XvjuOoKzA==";

        static int Main(string[] args)
        {
            try
            {
                MainAsync().Wait();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        private static async Task MainAsync()
        {
            int threads = 64;

            // Start generation of events
            var cts = new CancellationTokenSource();

            // Create a block with an asynchronous action
            var block = new ActionBlock<int>(
                async x => await StartEventGenerationAsync(cts.Token),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = threads
                });

            for (int i = 0; i < threads; i++)
            {
                await block.SendAsync(i);
            }

            block.Complete();
            await block.Completion;

            Console.ReadLine();
            cts.Cancel();
        }

        private static async Task StartEventGenerationAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        string cardNumber = GetUniqueToken(16, "0123456789");
                        decimal amount = decimal.Parse(DateTime.UtcNow.ToString("ss.ff"));

                        // Set payment
                        Payment payment = new Payment()
                        {
                            CardNumber = cardNumber,
                            ExpiryYear = 2019,
                            ExpiryMonth = 8,
                            Amount = amount,
                            CurrencyCode = "GBP",
                            CVV = 123
                        };

                        string setUrl = baseUrl + "?code=" + code;

                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage httpSetResponseMessage = await client.PostAsJsonAsync(setUrl, payment);
                            PaymentResponse paymentResponse = await httpSetResponseMessage.Content.ReadAsAsync<PaymentResponse>();

                            // Get payment
                            // Get only works locally as in-memory payment store not persisted accross function instances
                            // this would be managed by acquiring bank anyway and stored in a persisted database so not a problem so purposes of this example.
                            //string getUrl = baseUrl + paymentResponse.Id + "?code=" + code;

                            //HttpResponseMessage httpGetResponseMessage = await client.GetAsync(getUrl);
                            //PaymentEntityMasked paymentEntityMasked = await httpGetResponseMessage.Content.ReadAsAsync<PaymentEntityMasked>();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error sending payments. Exception: {0}", ex);
                }

                await Task.Delay(100, cancellationToken);
            }
        }

        public static string GetUniqueToken(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];

                // If chars.Length isn't a power of 2 then there is a bias if we simply use the modulus operator. The first characters of chars will be more probable than the last ones.
                // buffer used if we encounter an unusable random byte. We will regenerate it in this buffer
                byte[] buffer = null;

                // Maximum random number that can be used without introducing a bias
                int maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                crypto.GetBytes(data);

                char[] result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    byte value = data[i];

                    while (value > maxRandom)
                    {
                        if (buffer == null)
                        {
                            buffer = new byte[1];
                        }

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = chars[value % chars.Length];
                }

                return new string(result);
            }
        }
    }
}
