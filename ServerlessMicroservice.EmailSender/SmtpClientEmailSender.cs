using System.Net;
using System.Net.Mail;
using System.Text;

namespace ServerlessMicroservice.EmailSender
{
    public class SmtpClientEmailSender : IEmailSender
    {
        private readonly IMailConfig _mailConfig;
        private readonly ISmtpConfig _smtpConfig;

        public SmtpClientEmailSender(IMailConfig mailConfig, ISmtpConfig smtpConfig)
        {
            _mailConfig = mailConfig;
            _smtpConfig = smtpConfig;
        }

        public void SendMail(SendEmailModel model)
        {
            using (var emailMessage = new MailMessage())
            {
                emailMessage.To.Add(model.ToMailAddress);

                emailMessage.From = _mailConfig.FromMailAddress;
                emailMessage.Subject = model.MailSubject;
                emailMessage.Body = model.MailBody;
                emailMessage.IsBodyHtml = true;
                emailMessage.BodyEncoding = Encoding.Unicode;

                using (var smtpClient = new SmtpClient(_smtpConfig.SmtpServerHost))
                {
                    smtpClient.Port = _smtpConfig.SmtpPort;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(_smtpConfig.SmtpUserName, _smtpConfig.SmtpUserPassword);

                    smtpClient.Send(emailMessage);
                }
            }
        }
    }
}