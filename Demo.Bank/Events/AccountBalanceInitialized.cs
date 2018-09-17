using Demo.Command.Commands;

namespace Demo.Bank.Events
{
    public class AccountBalanceInitialized : IDomainEvent
    {
        public string AccuuntNumber { get; set; }
        public decimal Balance { get; set; }
        public string CausationId { get; set; }
    }
}