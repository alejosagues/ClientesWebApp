using System.Threading.Tasks;
using Clientes.API.MailServices.DTOs;

namespace Clientes.API.MailServices.Interfaces
{
    public interface IEmail
    {
        Task Send(string emailAddress, string body, EmailOptionsDTO options);
    }
}