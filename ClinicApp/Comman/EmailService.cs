using System.Net;
using System.Net.Mail;

namespace ClinicApp.Comman
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string from, string to, string subject, string messageText)
        {
            if (from is null) from = "Kareembakr63@gmail.com";
            
            var FromUser = new MailAddress(from);
            var fromPassword = "wmkd cckx vqnq evki";
            
            var message = new MailMessage() ;
            message.From = FromUser;
            message.Subject = subject;
            message.Body = $"<html><body>{messageText}</body></html>";
            message.IsBodyHtml = true ;
            message.To.Add(new MailAddress(to));

            var stmp = new SmtpClient("smtp.gmail.com" , 587);
            stmp.UseDefaultCredentials = false;
            stmp.Credentials = new NetworkCredential(from, fromPassword);
            stmp.EnableSsl = true;
            await stmp.SendMailAsync(message);

        }

 
    }

    public interface IEmailService
    {
        Task SendAsync(
            string from,
            string to,
            string subject,
            string messageText);
    }
}
