using Milochau.Emails.DataAccess;
using Milochau.Emails.Models.Options;
using Milochau.Emails.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SendGrid;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.FeatureManagement;
using Milochau.Core.Functions;

namespace Milochau.Emails.Tests
{
    [TestClass]
    public class StartupTests
    {
        private readonly ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        [TestMethod]
        public void ConfigureServices_Should_RegisterAllServices_When_Called()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "SendGrid:Key", "Key" }
            });

            var configuration = configurationBuilder.Build();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton(Mock.Of<IHostEnvironment>());
            serviceCollection.AddSingleton(Mock.Of<HtmlEncoder>());

            var startup = TestableStartup.Create<Startup>(configuration);

            // Act
            startup.ConfigureServices(serviceCollection);

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider.GetService<IOptions<EmailsOptions>>());
            Assert.IsNotNull(serviceProvider.GetService<IOptions<SendGridOptions>>());

            Assert.IsNotNull(serviceProvider.GetService<IEmailsService>());

            Assert.IsNotNull(serviceProvider.GetService<IEmailsDataAccess>());
            Assert.IsNotNull(serviceProvider.GetService<ISendGridClient>());
        }

        public class TestableStartup : Startup
        {
        }
    }
}
