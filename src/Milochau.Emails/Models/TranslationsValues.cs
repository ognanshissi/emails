using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Milochau.Emails.Models
{
    [Serializable]
    public class TranslationsValues : Dictionary<TranslationKey, string>
    {
        public TranslationsValues()
        {
        }

        protected TranslationsValues(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
