namespace Milochau.Emails.Services.EmailTemplates
{
    public interface IEmailTemplateFactory
    {
        IEmailTemplate Create(string templateId);
    }
}