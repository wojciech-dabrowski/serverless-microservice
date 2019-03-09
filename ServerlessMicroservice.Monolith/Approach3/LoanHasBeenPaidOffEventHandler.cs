using ServerlessMicroservice.EmailSender;
using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach3
{
    public class LoanHasBeenPaidOffEventHandler : IEventHandler<LoanHasBeenPaidOffEvent>
    {
        private const string MailSubject = "Your loan has been paid off";
        private readonly IEmailSender emailSender;

        public LoanHasBeenPaidOffEventHandler(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public void Handle(LoanHasBeenPaidOffEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\n" +
                           $"Your loan for {@event.LoanAmount} euro has been paid off.";
            var sendEmailModel = new SendEmailModel(@event.CustomerMailAddress, MailSubject, mailBody);
            emailSender.SendMail(sendEmailModel);
        }
    }
}