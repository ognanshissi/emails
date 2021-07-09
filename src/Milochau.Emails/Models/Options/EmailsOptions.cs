using System.Collections.Generic;

namespace Milochau.Emails.Models.Options
{
    public class EmailsOptions
    {
        public ICollection<string> AuthorizedRecipientHosts { get; set; } = new List<string>();
    }
}
