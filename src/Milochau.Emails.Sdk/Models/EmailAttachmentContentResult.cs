using System;

namespace Milochau.Emails.Sdk.Models
{
    /// <summary>Result of the storage of a <see cref="EmailAttachmentContent"/></summary>
    public class EmailAttachmentContentResult
    {
        /// <summary>File name</summary>
        public string FileName { get; set; }

        /// <summary>File URI</summary>
        public Uri Uri { get; set; }
    }
}
