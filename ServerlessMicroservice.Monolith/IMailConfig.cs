using System.Net.Mail;

namespace ServerlessMicroservice.Monolith
{
    public interface IMailConfig
    {
        MailAddress FromMailAddress { get; }
    }
}