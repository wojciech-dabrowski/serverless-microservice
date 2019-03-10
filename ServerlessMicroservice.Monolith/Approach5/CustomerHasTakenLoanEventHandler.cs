using ServerlessMicroservice.EmailSender;
using ServerlessMicroservice.Framework.Events;
using ServerlessMicroservice.Monolith.Events;

namespace ServerlessMicroservice.Monolith.Approach5
{
    public class CustomerHasTakenLoanEventHandler : IEventHandler<CustomerHasTakenLoanEvent>
    {
        private const string MailSubject = "You have taken a loan";

        public CustomerHasTakenLoanEventHandler()
        {
            //TODO
        }

        public void Handle(CustomerHasTakenLoanEvent @event)
        {
            // Some logic (maybe business as well) related with actions when customer has taken loan

            var mailBody = $"Hi, {@event.CustomerFirstName}\n\n" +
                           $"You have taken a loan for {@event.LoanAmount} euro.";
            var sendEmailModel = new SendEmailModel(@event.CustomerMailAddress, MailSubject, mailBody);
//            emailSender.SendMail(sendEmailModel);
        }
    }
}