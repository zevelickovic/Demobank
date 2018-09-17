using Demo.Command.Commands;

namespace Demo.Bank.Events
{
    public class TransactionCommitted : IDomainEvent, IAccountTransaction
    {
        public string TransactionId { get; set; }
        public string CausationId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}