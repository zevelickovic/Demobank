using Demo.Command.Commands;

namespace Demo.Bank.Commands
{
    public class CommitTransaction : ICommand
    {
        public string CausationId { get; }
        public string RoutingKey => AccountNumber;
        public string TransactionId { get; set; }
        public string AccountNumber { get; set; }

        public CommitTransaction()
        {
            
        }
        public CommitTransaction(string transactionId, string accountNumber, string causationId)
        {
            AccountNumber = accountNumber;
            TransactionId = transactionId;
            CausationId = causationId;
        }
    }
}