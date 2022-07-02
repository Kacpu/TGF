using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TGF.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public void SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = "kacpert757@gmail.com";
            string fromPassword = "phhnfvomtnxfbzsg"; //hasło wygenerować dla poczty z google -> hasła do aplikacji

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromMail);
            mail.Subject = subject;
            mail.To.Add(new MailAddress(email));
            mail.Body = "<html><body> " + htmlMessage + " </body></html>";
            mail.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
