using ServerlessMicroservice.EmailSender;
using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach4
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

            // NOTE: In this example it looks exactly the same as Approach#3,
            // but in this case dependencies should be installed via NuGet instead of direct referring EmailSender project 

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\n" +
                           $"Your loan for {@event.LoanAmount} {@event.LoanCurrency} has been paid off.";
            var sendEmailModel = new SendEmailModel(@event.CustomerMailAddress, MailSubject, mailBody);
            emailSender.SendMail(sendEmailModel);
        }
    }
}