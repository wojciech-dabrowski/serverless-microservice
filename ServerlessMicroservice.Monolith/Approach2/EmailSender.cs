using System.Net;
using System.Net.Mail;
using System.Text;

namespace ServerlessMicroservice.Monolith.Approach2
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

        public void SendMail(string toMailAddress, string mailSubject, string mailBody)
        {
            var smtpSend = new SmtpClient(smtpConfig.SmtpServerHost);

            using (var emailMessage = new MailMessage())
            {
                emailMessage.To.Add(toMailAddress);

                emailMessage.From = mailConfig.FromMailAddress;
                emailMessage.Subject = mailSubject;
                emailMessage.Body = mailBody;
                emailMessage.IsBodyHtml = true;
                emailMessage.BodyEncoding = Encoding.Unicode;

                smtpSend.Port = smtpConfig.SmtpPort;
                smtpSend.UseDefaultCredentials = false;
                smtpSend.Credentials = new NetworkCredential(smtpConfig.SmtpUserName, smtpConfig.SmtpUserPassword);

                smtpSend.Send(emailMessage);
            }
        }
    }
}