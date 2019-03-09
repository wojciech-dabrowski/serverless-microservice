using ServerlessMicroservice.Framework.Events;

namespace ServerlessMicroservice.Monolith.Events
{
    public class CustomerHasTakenLoanEvent : IEvent
    {
        public CustomerHasTakenLoanEvent(string customerMailAddress, string customerFirstName, decimal loanAmount)
        {
            CustomerMailAddress = customerMailAddress;
            CustomerFirstName = customerFirstName;
            LoanAmount = loanAmount;
        }

        public string CustomerMailAddress { get; }
        public string CustomerFirstName { get; }
        public decimal LoanAmount { get; }
    }
}