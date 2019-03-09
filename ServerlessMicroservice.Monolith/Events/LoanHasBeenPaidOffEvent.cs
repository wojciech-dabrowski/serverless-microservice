using ServerlessMicroservice.Framework.Events;

namespace ServerlessMicroservice.Monolith.Events
{
    public class LoanHasBeenPaidOffEvent : IEvent
    {
        public LoanHasBeenPaidOffEvent(string customerFirstName, string customerMailAddress, decimal loanAmount)
        {
            CustomerFirstName = customerFirstName;
            CustomerMailAddress = customerMailAddress;
            LoanAmount = loanAmount;
        }

        public string CustomerFirstName { get; }
        public string CustomerMailAddress { get; }
        public decimal LoanAmount { get; }
    }
}