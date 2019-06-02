using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Approach2.EmailSender;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach2
{
    public class CustomerHasTakenLoanEventHandler : IEventHandler<CustomerHasTakenLoanEvent>
    {
        private const string MailSubject = "You have taken a loan";
        private readonly IEmailSender _emailSender;

        public CustomerHasTakenLoanEventHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void Handle(CustomerHasTakenLoanEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\n" +
                           $"You have taken a loan for {@event.LoanAmount} {@event.LoanCurrency}.";
            var sendEmailModel = new SendEmailModel(@event.CustomerMailAddress, MailSubject, mailBody);
            _emailSender.SendMail(sendEmailModel);
        }
    }
}