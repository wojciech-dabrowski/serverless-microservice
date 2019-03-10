using System.Net;
using System.Net.Mail;
using System.Text;

namespace ServerlessMicroservice.Monolith.Approach2.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IMailConfig mailConfig;
        private readonly ISmtpConfig smtpConfig;

        public EmailSender(IMailConfig mailConfig, ISmtpConfig smtpConfig)
        {
            this.mailConfig = mailConfig;
            this.smtpConfig = smtpConfig;
        }

        public void SendMail(SendEmailModel model)
        {
            using (var emailMessage = new MailMessage())
            {
                emailMessage.To.Add(model.ToMailAddress);

                emailMessage.From = mailConfig.FromMailAddress;
                emailMessage.Subject = model.MailSubject;
                emailMessage.Body = model.MailBody;
                emailMessage.IsBodyHtml = true;
                emailMessage.BodyEncoding = Encoding.Unicode;

                using (var smtpClient = new SmtpClient(smtpConfig.SmtpServerHost))
                {
                    smtpClient.Port = smtpConfig.SmtpPort;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpConfig.SmtpUserName, smtpConfig.SmtpUserPassword);

                    smtpClient.Send(emailMessage);
                }
            }
        }
    }
}