using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.Utility.Convertor;
using Ghasedak.Core;
using Microsoft.Extensions.Configuration;

namespace Digikala.Core.Classes
{
    public class MessageSender
    {
        private readonly IConfiguration _configuration;
        private readonly IViewRenderService _viewRender;

        public MessageSender(IConfiguration configuration, IViewRenderService viewRender)
        {
            _configuration = configuration;
            _viewRender = viewRender;
        }
        public async Task Sms(string to, string body)
        {
            try
            {
                var sms = new Api("41dab1cfc8c70420f49b49f01080f87f63b632d922ea257ba2df54100a4c3df7");
                await sms.SendSMSAsync(body, to, "210002100");
            }
            catch (Ghasedak.Core.Exceptions.ApiException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Ghasedak.Core.Exceptions.ConnectionException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task Mail(MailMessage message)
        {
            using var smtpServer = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = Convert.ToInt32(_configuration["Smtp:Port"]),
                Credentials = new System.Net.NetworkCredential(_configuration["Smtp:Username"],
                    _configuration["Smtp:Password"]),
                EnableSsl = false
            };
            try
            {
                await smtpServer.SendMailAsync(message);
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            }
        }
        public async Task SendMailToUserWithView(string subject, User user, string viewName, string backUrl)
        {
            var message = new MailMessage
            {
                Subject = subject,
                Body = _viewRender.RenderToStringAsync(viewName, new Tuple<User, string>(user, backUrl)),
                From = new MailAddress(_configuration["Smtp:Username"], "شرکت دیجی کالا"),
                IsBodyHtml = true,
            };
            message.To.Add(user.Email);
            await Mail(message);
        }
    }
}