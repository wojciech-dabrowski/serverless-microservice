using System.Net;
using System.Net.Mail;
using System.Text;
using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach1
{
    public class LoanHasBeenPaidOffEventHandler : IEventHandler<LoanHasBeenPaidOffEvent>
    {
        private readonly IMailConfig mailConfig;
        private readonly ISmtpConfig smtpConfig;
        private const string MailSubject = "Your loan has been paid off";

        public LoanHasBeenPaidOffEventHandler(IMailConfig mailConfig, ISmtpConfig smtpConfig)
        {
            this.mailConfig = mailConfig;
            this.smtpConfig = smtpConfig;
        }
        
        public void Handle(LoanHasBeenPaidOffEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\nYour loan for {@event.LoanAmount} euro has been paid off.";
            SendMail(@event.CustomerMailAddress, mailBody);
        }
        
        private void SendMail(string toMailAddress, string mailBody)
        {
            var smtpSend = new SmtpClient(smtpConfig.SmtpServerHost);

            using (var emailMessage = new MailMessage())
            {
                emailMessage.To.Add(toMailAddress);

                emailMessage.From = mailConfig.FromMailAddress;
                emailMessage.Subject = MailSubject;
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