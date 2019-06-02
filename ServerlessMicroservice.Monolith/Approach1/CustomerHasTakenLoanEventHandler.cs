using System.Net;
using System.Net.Mail;
using System.Text;
using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach1
{
    public class CustomerHasTakenLoanEventHandler : IEventHandler<CustomerHasTakenLoanEvent>
    {
        private const string MailSubject = "You have taken a loan";
        private readonly IMailConfig _mailConfig;
        private readonly ISmtpConfig _smtpConfig;

        public CustomerHasTakenLoanEventHandler(ISmtpConfig smtpConfig, IMailConfig mailConfig)
        {
            _smtpConfig = smtpConfig;
            _mailConfig = mailConfig;
        }

        public void Handle(CustomerHasTakenLoanEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\nYou have taken a loan for {@event.LoanAmount} {@event.LoanCurrency}.";
            SendMail(@event.CustomerMailAddress, mailBody);
        }

        private void SendMail(string toMailAddress, string mailBody)
        {
            using (var emailMessage = new MailMessage())
            {
                emailMessage.To.Add(toMailAddress);

                emailMessage.From = _mailConfig.FromMailAddress;
                emailMessage.Subject = MailSubject;
                emailMessage.Body = mailBody;
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
