using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Toast.Common.Configurations;
using Toast.Sms.Configurations;
using Toast.Sms.Models;
using Toast.Sms.Validators;

[assembly: FunctionsStartup(typeof(Toast.Sms.Startup))]

namespace Toast.Sms
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
                                        .Get<ToastSettings<SmsEndpointSettings>>(ToastSettings.Name);
            services.AddSingleton(toastSettings);
        }

        private static void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient("messages");
        }

        private static void ConfigureValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<GetMessageRequestQueries>, GetMessageRequestQueryValidator>();
            services.AddScoped<IValidator<ListMessagesRequestQueries>, ListMessagesRequestQueryValidator>();
            services.AddScoped<IValidator<ListMessageStatusRequestQueries>, ListMessageStatusRequestQueryValidator>();
            services.AddScoped<IValidator<SendMessagesRequestBody>, SendMessagesRequestBodyValidator>();
            services.AddScoped<IValidator<ListSendersRequestQueries>, ListSendersRequestQueryValidator>();
        }
    }
}