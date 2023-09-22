using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Humanizer;
using MailKit.Security;
using HomeEntertainmentAdvisor.Models.Options;

namespace HomeEntertainmentAdvisor.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SMTPSettings settings;

        public EmailSender(SMTPSettings settings)
        {
            this.settings=settings;
        }
        /// <summary>
        /// Sends email to specified address, with specified subject and html content 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var message = new MimeMessage())
            {
                message.Sender=MailboxAddress.Parse(settings.Mail);
                message.Subject=subject;
                message.To.Add(MailboxAddress.Parse(email));
                message.Body=GetMessageBody(htmlMessage);

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtp.ConnectAsync(settings.Host, settings.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(settings.Mail, settings.Password);
                    await smtp.SendAsync(message);
                    smtp.Disconnect(true);
                }
            }
        }

        private MimeEntity GetMessageBody(string htmlMessage)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            return builder.ToMessageBody();
        }
    }
}
