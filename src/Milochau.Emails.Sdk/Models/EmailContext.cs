namespace Milochau.Emails.Sdk.Models
{
    /// <summary>Email context</summary>
    public class EmailContext
    {
        /// <summary>Application name</summary>
        public string ApplicationName { get; set; }

        /// <summary>Client application home page URL</summary>
        public string HomeUrl { get; set; }

        /// <summary>Client application privacy page URL</summary>
        public string PrivacyUrl { get; set; }

        /// <summary>Client application unsubscribe page URL</summary>
        public string UnsubscribeUrl { get; set; }

        /// <summary>Signature</summary>
        public EmailSignature Signature { get; set; } = new EmailSignature();

        /// <summary>Culture</summary>
        public TypeCulture Culture { get; set; }
    }
}
