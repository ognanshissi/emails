using Milochau.Emails.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Milochau.Emails.Models
{
    [Serializable]
    public class TranslationsOptions : Dictionary<TypeCulture, TranslationsValues>
    {
        public TranslationsOptions()
        {
        }

        protected TranslationsOptions(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
