using ServerlessMicroservice.EmailSenderMicroservice.SDK;
using ServerlessMicroservice.EmailSenderMicroservice.SDK.Client;
using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach5
{
    public class LoanHasBeenPaidOffEventHandler : IEventHandler<LoanHasBeenPaidOffEvent>
    {
        private readonly IEmailClient emailClient;
        private const string MailSubject = "Your loan has been paid off";

        public LoanHasBeenPaidOffEventHandler(IEmailClient emailClient)
        {
            this.emailClient = emailClient;
        }

        public void Handle(LoanHasBeenPaidOffEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\nYour loan for {@event.LoanAmount} {@event.LoanCurrency} has been paid off.";

            var insertMailModel = new InsertEmailModel
            {
                To = @event.CustomerMailAddress,
                Subject = MailSubject,
                Body = mailBody
            };

            emailClient.InsertEmail(insertMailModel);
        }
    }
}
