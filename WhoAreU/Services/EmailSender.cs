using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace WhoAreU.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(subject, "giuseppefacchi1@gmail.com"));
            mail.To.Add(new MailboxAddress(email));
            mail.Subject = subject;
            mail.Body = new TextPart("html")
            {
                Text = message
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, false);
                smtpClient.Authenticate("giuseppefacchi1@gmail.com", "bxnsslxcnduhtmye");
                smtpClient.Send(mail);
                smtpClient.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
