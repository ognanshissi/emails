using Milochau.Emails.Sdk.Models;
using Milochau.Emails.Models;

namespace Milochau.Emails.Services
{
    public interface ITranslationService
    {
        string Translate(TranslationKey key, TypeCulture culture, params string[] args);
    }
}
