using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using ServerlessMicroservice.Framework;
using ServerlessMicroservice.Framework.Event;

namespace ServerlessMicroservice.Monolith.EventHandlers
{
    public class CustomerHasTakenLoanEventHandler : IEventHandler<CustomerHasTakenLoanEvent>
    {
        public void Handle(CustomerHasTakenLoanEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            // Send e-mail to him with confirmation
        }

        private void SendMail(string fromEmail, string fromName, string toAddress,
                              string toName, string ccAddress, string subject, string body
        )
        {
            SendMail(new MailAddress(fromEmail, fromName), toAddress, ccAddress, subject, body);
        }

        private static void SendMail(MailAddress fromAddress, string toAddress, string ccAddress,
                                    string subject, string body)
        {
            var smtpSend = new SmtpClient(SmtpConfig.SmtpServerHost);

            using (var emailMessage = new MailMessage())
            {
                emailMessage.To.Add(toAddress);

                if (!String.IsNullOrEmpty(ccAddress))
                {
                    emailMessage.CC.Add(ccAddress);
                }

                emailMessage.From = fromAddress;
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                if (!Regex.IsMatch(emailMessage.Body, @"^([0-9a-z!@#\$\%\^&\*\(\)\-=_\+])", RegexOptions.IgnoreCase) ||
                    !Regex.IsMatch(emailMessage.Subject, @"^([0-9a-z!@#\$\%\^&\*\(\)\-=_\+])", RegexOptions.IgnoreCase))
                {
                    emailMessage.BodyEncoding = Encoding.Unicode;
                }

                if (String.IsNullOrWhiteSpace(SmtpConfig.SmtpUserName) 
                    && String.IsNullOrWhiteSpace(SmtpConfig.SmtpUserPassword)
                    && SmtpConfig.SmtpPort > 0)
                {
                    smtpSend.Port = SmtpConfig.SmtpPort;
                    smtpSend.UseDefaultCredentials = false;
                    smtpSend.Credentials = new NetworkCredential(SmtpConfig.SmtpUserName, SmtpConfig.SmtpUserPassword);
                }
            }
        }
    }
}