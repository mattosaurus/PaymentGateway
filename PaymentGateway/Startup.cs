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
            // Ideally enrich with additional properties to make for easier debugging
            // e.g. .WriteTo.AzureTableStorageWithProperties(storageAccount, propertyColumns: new string[] { "MS_FunctionInvocationId" }, storageTableName: Environment.GetEnvironmentVariable("RequestLogTable"), restrictedToMinimumLevel: LogEventLevel.Warning)
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
            // Add services via DI
            services
                .AddLogging(loggingBuilder =>
                    loggingBuilder.AddSerilog(dispose: true)
                )
                // Add the acquiring bank client as a singleton for this to allow all threads access to the shared dictionary
                // In real life this will depend on how the acquiring bank is being called but shared in-memory access won't be needed
                .AddSingleton<IAcquiringBankClient, AcquiringBankClient>()
                .AddSingleton<IAcquiringBankService, AcquiringBankService>()
                .AddScoped<IPaymentGatewayClient, PaymentGatewayClient>()
                .AddScoped<IPaymentGatewayService, PaymentGatewayService>();

            return services;
        }
    }
}
