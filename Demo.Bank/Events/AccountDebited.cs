using Demo.Command.Commands;

namespace Demo.Bank.Events
{
    public class AccountDebited : IDomainEvent, IAccountTransaction
    {
        public string AccuontNumber { get; set; }
        public string TransactionId { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string CausationId { get; set; }
    }
}