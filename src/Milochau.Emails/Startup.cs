using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Milochau.Core.Functions;
using Milochau.Emails.Sdk.Helpers;
using Milochau.Emails;
using Milochau.Emails.DataAccess;
using Milochau.Emails.Models.Options;
using Milochau.Emails.Services;
using Milochau.Emails.Services.EmailTemplates;
using SendGrid;
using System.Text.Encodings.Web;
using Milochau.Core.Abstractions;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Milochau.Emails
{
    public class Startup : CoreFunctionsStartup
    {
        protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            RegisterOptions(services);
            RegisterServices(services);
            RegisterDataAccess(services);
        }

        private void RegisterOptions(IServiceCollection services)
        {
            services.AddOptions<EmailsOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind("Emails", settings);
                });
            services.AddOptions<SendGridOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind("SendGrid", settings);
                });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddFeatureManagement();
            services.AddSingleton(StartupConfiguration.ConfigurationRefresher);

            services.AddScoped<IEmailsService, EmailsService>();
            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<IEmailTemplateFactory, EmailTemplateFactory>();

            services.AddScoped<IEmailsValidationHelper, EmailsValidationHelper>();
        }

        private void RegisterDataAccess(IServiceCollection services)
        {
            services.AddSingleton<IEmailsDataAccess, EmailsSendGridClient>();

            services.AddSingleton<IStorageDataAccess>(serviceProvider =>
            {
                var hostOptions = serviceProvider.GetService<IOptions<CoreHostOptions>>();
                var emailsOptions = serviceProvider.GetService<IOptions<EmailsOptions>>().Value;
                var logger = serviceProvider.GetRequiredService<ILogger<StorageDataAccess>>();
                return new StorageDataAccess(hostOptions, emailsOptions, logger);
            });
            
            services.AddSingleton<ISendGridClient>(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<SendGridOptions>>().Value;
                var sendGridKey = options.Key;
                return new SendGridClient(sendGridKey);
            });

            services.AddSingleton(HtmlEncoder.Default);
        }
    }
}
