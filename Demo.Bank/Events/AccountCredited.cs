using Demo.Command.Commands;

namespace Demo.Bank.Events
{
    public class AccountCredited : IDomainEvent, IAccountTransaction
    {
        public string AccountNumber { get; set; }
        public string FromAccount { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string CausationId { get; set; }
    }
}