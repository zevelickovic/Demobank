using Demo.Command.Commands;

namespace Demo.Bank.Commands
{
    public class Deposit : ICommand
    {
        public string TransactionId { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string CausationId { get; private set; }
        public string RoutingKey => ToAccount;

        public Deposit()
        {
            
        }
        public Deposit(string transactionId, string fromAccount, string toAccount, decimal amount, string causationId)
        {
            TransactionId = transactionId;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Amount = amount;
            CausationId = causationId;
        }
    }

}