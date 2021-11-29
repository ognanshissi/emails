namespace Milochau.Emails.Sdk.Models
{
    /// <summary>Email attachment</summary>
    public class EmailAttachment
    {
        /// <summary>File name</summary>
        /// <remarks>This must be the exact file name, as you can find in the storage</remarks>
        public string FileName { get; set; }

        /// <summary>Public file name</summary>
        /// <remarks>If a <see cref="PublicFileName"/> is not provided, the <see cref="FileName"/> will be used</remarks>
        public string PublicFileName { get; set; }
    }
}