using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace cfeya_webapp.Email
{
    public class SendEmail : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SendEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_configuration.GetSection("MailInfo:From").Value);
                mail.Subject = subject;
                mail.Body = htmlMessage;
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(email));
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Host = _configuration.GetSection("MailInfo:Host").Value;
                client.EnableSsl = true;
                NetworkCredential network = new NetworkCredential();
                network.UserName = _configuration.GetSection("MailInfo:From").Value;
                network.Password = _configuration.GetSection("MailInfo:Password").Value;
                client.Credentials = network;
                client.Port = Int32.Parse(_configuration.GetSection("MailInfo:Port").Value);
                await client.SendMailAsync(mail);
            }
        }
    }
}
