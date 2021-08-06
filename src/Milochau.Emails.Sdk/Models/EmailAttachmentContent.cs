using System.IO;

namespace Milochau.Emails.Sdk.Models
{
    /// <summary>Content of an email attachment</summary>
    public class EmailAttachmentContent
    {
        /// <summary>File content</summary>
        public Stream Content { get; set; }
    }
}
