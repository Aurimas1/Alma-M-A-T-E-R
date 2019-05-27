using API.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Services
{
    public class MailSender : IMailSender
    {
        private readonly MailOptions options;

        public MailSender(IOptions<MailOptions> options)
        {
            this.options = options.Value;
        }

        public void Send(string email, string name, string subject, string body)
        {
            var fromAddress = new MailAddress(options.Mail, "psk-no-reply");
            var toAddress = new MailAddress(email, name);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, options.Password)
            };
            Task.Run(() => 
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            });
        }
    }
}
