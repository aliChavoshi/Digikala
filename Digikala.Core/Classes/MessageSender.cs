using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Ghasedak.Core;
using Microsoft.Extensions.Configuration;

namespace Digikala.Core.Classes
{
    public class MessageSender
    {
        private readonly IConfiguration _configuration;

        public MessageSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task Sms(string to, string body)
        {
            try
            {
                var sms = new Api("41dab1cfc8c70420f49b49f01080f87f63b632d922ea257ba2df54100a4c3df7");
                await sms.SendSMSAsync(body, to, "210002100");
            }
            catch(Ghasedak.Core.Exceptions.ApiException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Ghasedak.Core.Exceptions.ConnectionException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task Mail(MailMessage message)
        {
            var smtpServer = new SmtpClient(_configuration["SendEmail:SmtpMailServer"]);
            message.From = new MailAddress(_configuration["SendEmail:Email"], "شرکت دیجی کالا");
            message.IsBodyHtml = true;
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential(_configuration["SendEmail:Email"], _configuration["OptionsEmail:Password"]);
            smtpServer.EnableSsl = false;

            await smtpServer.SendMailAsync(message);
        }
    }
}