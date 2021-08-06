using System;
using System.Collections.Generic;

namespace Milochau.Emails.Models.Options
{
    public class EmailsOptions
    {
        public ICollection<string> AuthorizedRecipientHosts { get; set; } = new List<string>();

        /// <summary>URI of the Storage Account used by the Emails microservice store emails attachments</summary>
        /// <remarks>Should be formatted as: https://xxx.blob.core.windows.net</remarks>
        public string StorageAccountUri { get; set; }
    }
}
