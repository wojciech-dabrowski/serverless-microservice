using ServerlessMicroservice.EmailSenderMicroservice.SDK;
using ServerlessMicroservice.EmailSenderMicroservice.SDK.Client;
using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach5
{
    public class CustomerHasTakenLoanEventHandler : IEventHandler<CustomerHasTakenLoanEvent>
    {
        private const string MailSubject = "You have taken a loan";
        private readonly IEmailClient emailClient;

        public CustomerHasTakenLoanEventHandler(IEmailClient emailClient)
        {
            this.emailClient = emailClient;
        }

        public void Handle(CustomerHasTakenLoanEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\nYou have taken a loan for {@event.LoanAmount} {@event.LoanCurrency}.";

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
