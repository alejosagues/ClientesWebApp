using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Clientes.API.MailServices.DTOs;
using Clientes.API.MailServices.Interfaces;

namespace Clientes.API.MailServices
{
    public class Mailjet : IEmail
    {
        public async Task Send(string emailAddress, string body, EmailOptionsDTO options)
        {
            //setup mail client
            var client = new SmtpClient();
            client.Host = options.Host;
            client.Credentials = new NetworkCredential(options.ApiKey, options.ApiKeySecret);
            client.Port = options.Port;
            //write html body
            var message = new MailMessage(options.SenderEmail, emailAddress);
            message.Body = body;
            message.IsBodyHtml = true;
            //send call
            await client.SendMailAsync(message);
        }
    }
}