using ServerlessMicroservice.Framework.Events;

namespace ServerlessMicroservice.Monolith.Events
{
    public class CustomerHasTakenLoanEvent : IEvent
    {
        public CustomerHasTakenLoanEvent(string customerMailAddress, string customerFirstName, decimal loanAmount, string loanCurrency)
        {
            CustomerMailAddress = customerMailAddress;
            CustomerFirstName = customerFirstName;
            LoanAmount = loanAmount;
            LoanCurrency = loanCurrency;
        }

        public string CustomerMailAddress { get; }
        public string CustomerFirstName { get; }
        public decimal LoanAmount { get; }
        public string LoanCurrency { get; }
    }
}