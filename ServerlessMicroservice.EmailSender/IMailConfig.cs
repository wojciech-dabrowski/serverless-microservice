using System.Net.Mail;

namespace ServerlessMicroservice.EmailSender
{
    public interface IMailConfig
    {
        MailAddress FromMailAddress { get; }
    }
}