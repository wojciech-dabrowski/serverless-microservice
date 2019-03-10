using ServerlessMicroservice.Framework.Events;

namespace ServerlessMicroservice.Monolith.Events
{
    public class LoanHasBeenPaidOffEvent : IEvent
    {
        public LoanHasBeenPaidOffEvent(string customerFirstName, string customerMailAddress, decimal loanAmount, string loanCurrency)
        {
            CustomerFirstName = customerFirstName;
            CustomerMailAddress = customerMailAddress;
            LoanAmount = loanAmount;
            LoanCurrency = loanCurrency;
        }

        public string CustomerFirstName { get; }
        public string CustomerMailAddress { get; }
        public decimal LoanAmount { get; }
        public string LoanCurrency { get; }
    }
}