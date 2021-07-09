using Milochau.Emails.Sdk.Models;
using Milochau.Emails.Models;
using System;
using System.Text.Encodings.Web;

namespace Milochau.Emails.Services.EmailTemplates
{
    public class DefaultEmailTemplate : IEmailTemplate
    {
        private readonly ITranslationService translationService;
        protected readonly HtmlEncoder htmlEncoder;

        public DefaultEmailTemplate(ITranslationService translationService,
            HtmlEncoder htmlEncoder)
        {
            this.translationService = translationService;
            this.htmlEncoder = htmlEncoder;
        }

        #region <head>

        private string GetHead(Email email)
        {
            return "<head>" +
                GetHeadMetadata(email) +
                GetHeadStyles() +
            "</head>";
        }
        private string GetHeadMetadata(Email email)
        {
            return @"
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
<meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
<meta name=""author"" content=""Milochau"">
<title>" + htmlEncoder.Encode(email.Subject) + @"</title>";
        }
        private string GetHeadStyles()
        {
            return @"
<style type=""text/css"">
    /* CLIENT-SPECIFIC STYLES */
    body, table, td, a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
    }

    table, td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
    }

    img {
        -ms-interpolation-mode: bicubic;
    }

    /* RESET STYLES */
    img {
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
    }

    table {
        border-collapse: collapse !important;
    }

    body {
        height: 100% !important;
        margin: 0 !important;
        padding: 0 !important;
        width: 100% !important;
    }

    /* iOS BLUE LINKS */
    a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
    }

    /* MEDIA QUERIES */
    media screen and (max-width: 480px) {
        .mobile-hide {
            display: none !important;
        }

        .mobile-center {
            text-align: center !important;
        }
    }

    /* ANDROID CENTER FIX */
    div[style*=""margin: 16px 0;""] {
        margin: 0 !important;
    }
</style>";
        }

        #endregion
        #region <body>

        private string GetBodyPreHeader(Email email)
        {
            string preheader;
            if (!string.IsNullOrEmpty(email.Context.ApplicationName))
            {
                preheader = $"{email.Context.ApplicationName} - {email.Subject}";
            }
            else
            {
                preheader = email.Subject;
            }

            return @"
<div style=""display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: Open Sans, Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"">" +
    htmlEncoder.Encode(preheader) +
@"</div>";
        }
        private string GetBodyHeader(Email email)
        {
            string header;
            if (!string.IsNullOrEmpty(email.Context.ApplicationName))
            {
                header = email.Context.ApplicationName;
            }
            else
            {
                header = email.Subject;
            }
            if (!string.IsNullOrEmpty(header))
            {
                var result = @"
<tr>
    <td align=""center"" valign=""top"" style=""font-size:0; padding: 35px;"" bgcolor=""#044767"">
        <div style=""display:inline-block; vertical-align:top; width:100%;"">
            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                <tr>
                    <td align=""center"" valign=""top"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 28px; font-weight: 800; line-height: 48px;"" class=""mobile-center"">";
                if (!string.IsNullOrEmpty(email.Context.HomeUrl))
                {
                    result += @"<a href=""" + email.Context.HomeUrl + @""" target=""_blank"" rel=""noopener"" style=""color: #ffffff; text-decoration: none;"">
                        <h1 style=""font-size: 28px; font-weight: 800; margin: 0; color: #ffffff;"">" + htmlEncoder.Encode(header) + @"</h1>
                    </a>";
                }
                else
                {
                    result += @"<h1 style=""font-size: 28px; font-weight: 800; margin: 0; color: #ffffff;"">" + htmlEncoder.Encode(header) + @"</h1>";
                }
                result += @"
                    </td>
                </tr>
            </table>
        </div>
    </td>
</tr>";
                return result;
            }
            else
            {
                return "";
            }
        }
        private string GetBodySubject(string content)
        {
            return @"
<tr>
    <td align=""center"" style=""padding: 15px 35px 20px 35px; background-color: #ffffff;"" bgcolor=""#ffffff"">
        <!--[if (gte mso 9)|(IE)]>
        <table align=""center"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""600"">
        <tr>
        <td align=""center"" valign=""top"" width=""600"">
        <![endif]-->
        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px;"">" +
            content + @"
        </table>
        <!--[if (gte mso 9)|(IE)]>
        </td>
        </tr>
        </table>
        <![endif]-->
    </td>
</tr>";
        }
        private string GetBodyCallToAction(Email email)
        {
            if (!string.IsNullOrEmpty(email.CallToAction.Url))
            {
                return @"
<tr>
    <td align=""center"" style="" padding: 25px; background-color: #1b9ba3;"" bgcolor=""#1b9ba3"">
        <!--[if (gte mso 9)|(IE)]>
        <table align=""center"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""600"">
        <tr>
        <td align=""center"" valign=""top"" width=""600"">
        <![endif]-->
        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px;"">" +
            /* Call to action description
                <tr>
                    <td align=""center"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding-top: 25px;"">
                        <h2 style=""font-size: 24px; font-weight: 800; line-height: 30px; color: #ffffff; margin: 0;"">
                            Get 25% off your next order.
                        </h2>
                    </td>
                </tr>
            */
           @"<tr>
                <td align=""center"" style=""padding: 5px 0 5px 0;"">
                    <table border=""0"" cellspacing=""0"" cellpadding=""0"">
                        <tr>
                            <td align=""center"" style=""border-radius: 5px;"" bgcolor=""#66b3b7"">
                                <a href=""" + email.CallToAction.Url + @""" target=""_blank"" rel=""noopener"" style=""font-size: 18px; font-family: Open Sans, Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; border-radius: 5px; background-color: #66b3b7; padding: 15px 30px; border: 1px solid #66b3b7; display: block;"">" + htmlEncoder.Encode(email.CallToAction.Title) + @"</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <!--[if (gte mso 9)|(IE)]>
        </td>
        </tr>
        </table>
        <![endif]-->
    </td>
</tr>";
            }
            else
            {
                return "";
            }
        }
        private string GetBodyFooter(Email email)
        {
            return @"
<tr>
    <td align=""center"" style=""padding: 28px; background-color: #ffffff;"" bgcolor=""#ffffff"">
        <!--[if (gte mso 9)|(IE)]>
        <table align=""center"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""600"">
        <tr>
        <td align=""center"" valign=""top"" width=""600"">
        <![endif]-->
        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px;"">" +
            GetBodyFooterSignature(email) +
            GetBodyFooterPrivacy(email) +
            GetBodyFooterUnsubscribe(email) +
            @"
        </table>
        <!--[if (gte mso 9)|(IE)]>
        </td>
        </tr>
        </table>
        <![endif]-->
    </td>
</tr>";
        }
        private string GetBodyFooterPrivacy(Email email)
        {
            var result = @"
<tr>
    <td align=""center"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 24px;"">
        <p style=""font-size: 14px; font-weight: 400; line-height: 20px; color: #777777;"">";
            if (!string.IsNullOrEmpty(email.Context.PrivacyUrl))
            {
                result += @"<a href=""" + email.Context.PrivacyUrl + @""" target=""_blank"" rel=""noopener"" style=""color: #777777;"">" + translationService.Translate(TranslationKey.EmailTemplate_Privacy, email.Context.Culture) + @"</a>&nbsp;&nbsp;";
            }
            if (email.ReplyTo != null && !string.IsNullOrEmpty(email.ReplyTo.Email))
            {
                result += @"<a href=""mailto:" + email.ReplyTo.Email + @""" target=""_blank"" rel=""noopener"" style=""color: #777777;"">" + translationService.Translate(TranslationKey.EmailTemplate_Contact, email.Context.Culture) + @"</a>";
            }
            result += @"
        </p>
        <p style=""margin: 0; font-size: 12px; font-weight: 400; line-height: 18px; color: #777777;"">
            &copy; 2018 - " + DateTime.Now.Year + @" — Antoine Milochau
        </p>
    </td>
</tr>";
            return result;
        }
        private string GetBodyFooterSignature(Email email)
        {
            string signatureName;
            if (!string.IsNullOrEmpty(email.Context.Signature.Name))
            {
                signatureName = email.Context.Signature.Name;
            }
            else
            {
                signatureName = translationService.Translate(TranslationKey.EmailTemplate_DefaultSignature, email.Context.Culture);
            }

            return @"
<tr>
    <td align=""center"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 24px; padding: 5px 0 10px 0;"">
        <p style=""font-size: 14px; font-weight: 800; line-height: 18px; color: #333333;"">" +
            signatureName +
        @"</p>
    </td>
</tr>";
        }
        private string GetBodyFooterUnsubscribe(Email email)
        {
            if (!string.IsNullOrEmpty(email.Context.UnsubscribeUrl))
            {
                return @"
<tr>
    <td align=""center"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 24px;"">
        <p style=""font-size: 14px; font-weight: 400; line-height: 20px; color: #777777;"">" +
            translationService.Translate(TranslationKey.EmailTemplate_Unsubscribe, email.Context.Culture, email.Context.UnsubscribeUrl) +
        @"</p>
    </td>
</tr>";
            }
            else
            {
                return "";
            }
        }

        #endregion

        public string GetAsString(Email email)
        {
            var content = GetContent(email);

            return @"<!DOCTYPE html>
<html>" +
GetHead(email) +
@"<body style=""margin: 0 !important; padding: 0 !important; background-color: #eeeeee;"" bgcolor=""#eeeeee"">" +
    GetBodyPreHeader(email) +
    @"<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
        <tr>
            <td align=""center"" style=""background-color: #eeeeee;"" bgcolor=""#eeeeee"">
                <!--[if (gte mso 9)|(IE)]>
                <table align=""center"" border=""0"" cellspacing=""0"" cellpadding=""0"" width=""600"">
                <tr>
                <td align=""center"" valign=""top"" width=""600"">
                <![endif]-->
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px;"">" +
                    GetBodyHeader(email) +
                    GetBodySubject(content) +
                    GetBodyCallToAction(email) +
                    GetBodyFooter(email) +
                    @"
                </table>
                <!--[if (gte mso 9)|(IE)]>
                </td>
                </tr>
                </table>
                <![endif]-->
            </td>
        </tr>
    </table>
</body>
</html>";
        }

        private string GetContent(Email email)
        {
            var message = GetMessage(email);
            if (email.Table != null)
            {
                message += GetTable(email.Table);
            }
            return message;
        }
        private string GetMessage(Email email)
        {
            return @"
<tr>
    <td align=""center"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding-top: 10px;"">
        <p style=""font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;"">" +
            (email.IsHtml ? email.Body : htmlEncoder.Encode(email.Body)) + @"
        </p>
    </td>
</tr>";
        }
        private string GetTable(EmailTable email)
        {
            if (email.Body != null)
            {
                var result = @"
<tr>
    <td align=""left"" style=""padding-top: 20px;"">
        <table cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">";
                if (!string.IsNullOrEmpty(email.Header))
                {
                    result += @"
                <tr>
                    <td colspan=""2"" align=""center"" bgcolor=""#eeeeee"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 800; line-height: 24px; padding: 10px;"">" +
                            htmlEncoder.Encode(email.Header) + @"
                    </td>
                </tr>";
                }
                foreach (var row in email.Body)
                {
                    result += @"
                <tr>
                    <td width=""50%"" align=""left"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px;"">" +
                            htmlEncoder.Encode(row.Item1) + @"
                    </td>
                    <td width=""50%"" align=""left"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px;"">" +
                            htmlEncoder.Encode(row.Item2) + @"
                    </td>
                </tr>";
                }
                result += GetFooter(email);
                result += @"
        </table>
    </td>
</tr>
";
                return result;
            }
            else
            {
                return "";
            }
        }
        private string GetFooter(EmailTable email)
        {
            if (email.Footer != null)
            {
                return @"
<tr>
    <td width=""50%"" align=""left"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 800; line-height: 24px; padding: 10px; border-top: 3px solid #eeeeee; border-bottom: 3px solid #eeeeee;"">" +
        htmlEncoder.Encode(email.Footer.Item1) + @"
    </td>
    <td width=""50%"" align=""left"" style=""font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 800; line-height: 24px; padding: 10px; border-top: 3px solid #eeeeee; border-bottom: 3px solid #eeeeee;"">" +
        htmlEncoder.Encode(email.Footer.Item2) + @"
    </td>
</tr>";
            }
            else
            {
                return "";
            }
        }
    }
}
