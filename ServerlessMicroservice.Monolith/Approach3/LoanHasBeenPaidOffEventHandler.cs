using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach3
{
    public class LoanHasBeenPaidOffEventHandler : IEventHandler<LoanHasBeenPaidOffEvent>
    {
        private const string MailSubject = "Your loan has been paid off";

        public LoanHasBeenPaidOffEventHandler()
        {
        }

        public void Handle(LoanHasBeenPaidOffEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\nYour loan for {@event.LoanAmount} euro has been paid off.";
            emailSender.SendMail(@event.CustomerMailAddress, MailSubject, mailBody);
        }
    }
}