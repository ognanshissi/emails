using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Encodings.Web;

namespace Milochau.Emails.Services.EmailTemplates
{
    public class EmailTemplateFactory : IEmailTemplateFactory
    {
        private readonly IServiceProvider sp;

        public EmailTemplateFactory(IServiceProvider sp)
        {
            this.sp = sp;
        }

        public IEmailTemplate Create(string templateId)
        {
            return templateId switch
            {
                "Wedding" => new WeddingEmailTemplate(sp.GetService<ITranslationService>(), sp.GetService<HtmlEncoder>()),
                _ => new DefaultEmailTemplate(sp.GetService<ITranslationService>(), sp.GetService<HtmlEncoder>()),
            };
        }
    }
}
