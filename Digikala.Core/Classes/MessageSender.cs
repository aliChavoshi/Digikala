using System.Threading.Tasks;
using Ghasedak.Core;

namespace Digikala.Core.Classes
{
    public class MessageSender
    {
        public async Task Sms(string to, string body)
        {
            var sms = new Api("41dab1cfc8c70420f49b49f01080f87f63b632d922ea257ba2df54100a4c3df7");
            await sms.SendSMSAsync(body, to, "210002100");
        }
    }
}