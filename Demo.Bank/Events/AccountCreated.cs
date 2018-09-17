using Demo.Command.Commands;

namespace Demo.Bank.Events
{
    public class AccountCreated: IDomainEvent
    {
        public string AccountNumber { get; set; }
        public string CausationId { get; set; }
    }
}