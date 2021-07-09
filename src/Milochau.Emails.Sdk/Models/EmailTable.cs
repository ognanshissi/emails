using System;
using System.Collections.Generic;

namespace Milochau.Emails.Sdk.Models
{
    /// <summary>Email table</summary>
    public class EmailTable
    {
        /// <summary>Table header</summary>
        public string Header { get; set; }

        /// <summary>Table body</summary>
        public List<Tuple<string, string>> Body { get; set; }

        /// <summary>Table footer (last row)</summary>
        public Tuple<string, string> Footer { get; set; }
    }
}
