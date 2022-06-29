using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Base
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string html);
    }
    public class EmailService : IEmailService
    {
        public readonly string EmailSender = "kari.simonis16@ethereal.email";
        public void Send(string from, string to, string subject, string html)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("kari.simonis16@ethereal.email", "9Q7JHmQHphGexU32By");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
