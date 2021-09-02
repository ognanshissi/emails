using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milochau.Core.Functions;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Emails
{
    public static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            new HostBuilder()
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    var environmentName = CoreOptionsFactory.GetCurrentEnvironmentFromEnvironmentVariables();
                    var hostName = CoreOptionsFactory.GetCurrentHostFromEnvironmentVariables();

                    configurationBuilder
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{hostName}.json", optional: true, reloadOnChange: false);
                })
                .ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    ConfigurationRegistration.AddApplicationConfiguration(webHostBuilderContext.Configuration, configurationBuilder);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var startup = CoreFunctionsStartup.Create<Startup>(hostContext.Configuration);
                    startup.ConfigureServices(services);
                    services.AddSingleton<CoreFunctionsStartup>(startup);
                })
                .ConfigureFunctionsWorkerDefaults((hostBuilderContext, functionsWorkerApplicationBuilder) =>
                {
                    var serviceProvider = functionsWorkerApplicationBuilder.Services.BuildServiceProvider();

                    var startup = serviceProvider.GetRequiredService<CoreFunctionsStartup>();

                    startup.Configure(serviceProvider, functionsWorkerApplicationBuilder);
                });
    }
}
