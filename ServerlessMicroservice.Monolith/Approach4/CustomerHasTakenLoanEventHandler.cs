using ServerlessMicroservice.EmailSender;
using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach4
{
    public class CustomerHasTakenLoanEventHandler : IEventHandler<CustomerHasTakenLoanEvent>
    {
        private const string MailSubject = "You have taken a loan";
        private readonly IEmailSender emailSender;

        public CustomerHasTakenLoanEventHandler(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public void Handle(CustomerHasTakenLoanEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            // NOTE: In this example it looks exactly the same as Approach#3,
            // but in this case dependencies should be installed via NuGet instead of direct referring EmailSender project 

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\n" +
                           $"You have taken a loan for {@event.LoanAmount} {@event.LoanCurrency}.";
            var sendEmailModel = new SendEmailModel(@event.CustomerMailAddress, MailSubject, mailBody);
            emailSender.SendMail(sendEmailModel);
        }
    }
}