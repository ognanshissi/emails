using Milochau.Emails.Sdk.Models;
using Milochau.Emails.Models;

namespace Milochau.Emails.Services
{
    public class TranslationService : ITranslationService
    {
        public string Translate(TranslationKey key, TypeCulture culture, params string[] args)
        {
            var dictionary = translationOptions[culture];
            if (dictionary.TryGetValue(key, out var textFromKey))
            {
                return string.Format(textFromKey, args);
            }
            else
            {
                return $"{key}";
            }
        }

        private static readonly TranslationsOptions translationOptions = new TranslationsOptions
        {
            [TypeCulture.English] = new TranslationsValues
            {
                [TranslationKey.EmailTemplate_Unsubscribe] = @"If these emails get annoying, please feel free to <a href=""{0}"" target=""_blank"" rel=""noopener"" style=""color: #777777;"">unsubscribe</a>.",
                [TranslationKey.EmailTemplate_Privacy] = "Privacy",
                [TranslationKey.EmailTemplate_Contact] = "Contact",
                [TranslationKey.EmailTemplate_DefaultSignature] = "The Milochau Team"
            },
            [TypeCulture.French] = new TranslationsValues
            {
                [TranslationKey.EmailTemplate_Unsubscribe] = @"Si ces emails vous ennuient, vous pouvez vous <a href=""{0}"" target=""_blank"" rel=""noopener"" style=""color: #777777;"">désabonner</a>.",
                [TranslationKey.EmailTemplate_Privacy] = "Confidentialité",
                [TranslationKey.EmailTemplate_Contact] = "Contact",
                [TranslationKey.EmailTemplate_DefaultSignature] = "La team Milochau"
            }
        };
    }
}
