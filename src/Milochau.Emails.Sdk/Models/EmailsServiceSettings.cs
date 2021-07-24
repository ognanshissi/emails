namespace Milochau.Emails.Sdk.Models
{
    /// <summary>Emails service settings</summary>
    public class EmailsServiceSettings
    {
        /// <summary>Namespage of the Service bus used by the Emails microservice</summary>
        /// <remarks>Should be formatted as: xxx.servicebus.windows.net</remarks>
        public string ServiceBusNamespace { get; set; }
    }
}
