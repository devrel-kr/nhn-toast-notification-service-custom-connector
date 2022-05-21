using FluentValidation;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Toast.Common.Configurations;
using Toast.Sms.Verification.Configurations;
using Toast.Sms.Verification.Models;
using Toast.Sms.Verification.Validators;

[assembly: FunctionsStartup(typeof(Toast.Sms.Verification.Startup))]

namespace Toast.Sms.Verification
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                   .AddEnvironmentVariables();

            base.ConfigureAppConfiguration(builder);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureAppSettings(builder.Services);
            ConfigureHttpClient(builder.Services);
            ConfigureValidators(builder.Services);
        }

        private static void ConfigureAppSettings(IServiceCollection services)
        {
            var toastSettings = services.BuildServiceProvider()
                                        .GetService<IConfiguration>()
                                        .Get<ToastSettings<SmsVerificationEndpointSettings>>(ToastSettings.Name);
            services.AddSingleton(toastSettings);
        }

        private static void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient("senders");
        }

        private static void ConfigureValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<ListSendersRequestQueries>, ListSendersRequestQueryValidator>();
        }
    }
}