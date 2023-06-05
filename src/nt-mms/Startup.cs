using System;

using FluentValidation;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Toast.Common.Configurations;
using Toast.Common.Validators;
using Toast.Mms.Configurations;
using Toast.Mms.Models;
using Toast.Mms.Validators;

[assembly: FunctionsStartup(typeof(Toast.Mms.Startup))]

namespace Toast.Mms
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
                                        .Get<ToastSettings<MmsEndpointSettings>>(ToastSettings.Name);
            services.AddSingleton(toastSettings);

            var options = new DefaultOpenApiConfigurationOptions()
            {
                OpenApiVersion = OpenApiVersionType.V3,
                Info = new OpenApiInfo()
                {
                    Version = "v1.0.0",
                    Title = "NHN Cloud MMS API Facade",
                    Description = "This is a facade API for sending MMS messages through the NHN Cloud Notification service.",
                }
            };
            /* ⬇️⬇️⬇️ for GH Codespaces ⬇️⬇️⬇️ */
            var codespaces = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__RunOnCodespaces"), out var isCodespaces) && isCodespaces;
            if (codespaces)
            {
                options.IncludeRequestingHostName = false;
            }
            /* ⬆️⬆️⬆️ for GH Codespaces ⬆️⬆️⬆️ */
            services.AddSingleton<IOpenApiConfigurationOptions>(options);
        }

        private static void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient("mms");
        }

        private static void ConfigureValidators(IServiceCollection services)
        {
            services.AddSingleton<IRegexDateTimeWrapper, RegexDateTimeWrapper>();
            //services.AddScoped<IValidator<GetMessageRequestQueries>, GetMessageRequestQueryValidator>();
            //services.AddScoped<IValidator<ListMessagesRequestQueries>, ListMessagesRequestQueryValidator>();
            //services.AddScoped<IValidator<ListMessageStatusRequestQueries>, ListMessageStatusRequestQueryValidator>();
            services.AddScoped<IValidator<SendMessagesRequestBody>, SendMessagesRequestBodyValidator>();
        }
    }
}