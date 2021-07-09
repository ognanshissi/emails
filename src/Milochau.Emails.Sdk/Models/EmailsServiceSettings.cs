namespace Milochau.Emails.Sdk.Models
{
    /// <summary>Emails service settings</summary>
    public class EmailsServiceSettings
    {
        /// <summary>Connection string to the Service bus used by the Emails microservice</summary>
        public string ServiceBusConnectionString { get; set; }

        /// <summary>Queue name for the Service Bus used by the Emails microservice</summary>
        public string ServiceBusQueueName { get; set; }
    }
}
