using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Models.Clients;
using PaymentGateway.Services;
using Serilog;
using System;

namespace PaymentGateway
{
    public class Startup : IWebJobsStartup
    {
        public Startup()
        {
            // Initialize serilog logger
            // Add other logging destinations here like table storage or SQL
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        public void Configure(IWebJobsBuilder builder)
        {
            ConfigureServices(builder.Services).BuildServiceProvider(true);
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging(loggingBuilder =>
                    loggingBuilder.AddSerilog(dispose: true)
                )
                .AddTransient<IPaymentGatewayClient, PaymentGatewayClient>()
                .AddTransient<IPaymentGatewayService, PaymentGatewayService>();

            return services;
        }
    }
}
