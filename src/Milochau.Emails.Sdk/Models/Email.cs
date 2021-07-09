using System.Collections.Generic;

namespace Milochau.Emails.Sdk.Models
{
    /// <summary>Email content and metadata</summary>
    public class Email
    {
        /// <summary>Recipients</summary>
        public List<EmailAddress> Tos { get; set; } = new List<EmailAddress>();

        /// <summary>Carbon Copies</summary>
        public List<EmailAddress> Ccs { get; set; } = new List<EmailAddress>();

        /// <summary>Blind Carbon Copies</summary>
        public List<EmailAddress> Bccs { get; set; } = new List<EmailAddress>();

        /// <summary>Sender</summary>
        public EmailAddress From { get; set; } = new EmailAddress();

        /// <summary>Where to reply</summary>
        public EmailAddress ReplyTo { get; set; } = new EmailAddress();

        /// <summary>Context</summary>
        public EmailContext Context { get; set; } = new EmailContext();

        /// <summary>Call to action</summary>
        public EmailCallToAction CallToAction { get; set; } = new EmailCallToAction();

        /// <summary>Table data</summary>
        public EmailTable Table { get; set; } = new EmailTable();

        /// <summary>Mail subject</summary>
        public string Subject { get; set; }

        /// <summary>Mail body</summary>
        public string Body { get; set; }

        /// <summary>Is <see cref="Body"/> HTML?</summary>
        public bool IsHtml { get; set; }

        /// <summary>Id of the template to use</summary>
        public string TemplateId { get; set; }

        /// <summary>Importance</summary>
        public ImportanceType Importance { get; set; }

        /// <summary>Priority</summary>
        public PriorityType Priority { get; set; }

        /// <summary>Mail attachements references</summary>
        public ICollection<StorageAttachment> Attachments { get; set; } = new List<StorageAttachment>();
    }
}
