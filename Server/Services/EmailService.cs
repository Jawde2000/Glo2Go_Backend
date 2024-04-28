using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Server.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _from;
        private readonly string _displayName;

        public EmailService(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient
            {
                Host = configuration["EmailSettings:MailServer"],
                Port = int.Parse(configuration["EmailSettings:MailPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential
                {
                    UserName = configuration["EmailSettings:Sender"],
                    Password = configuration["EmailSettings:Password"]
                }
            };

            _from = configuration["EmailSettings:Sender"];
            _displayName = configuration["EmailSettings:SenderName"];
        }


    }
}
