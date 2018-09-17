using Akka.Actor;
using Akka.Event;
using Demo.Bank.Commands;
using Demo.Bank.Events;

namespace Demo.Bank.Actors
{
    public class TransactionSaga : ReceiveActor
    {
        private readonly IActorRef _accountsRef;

        private string _transactionId;
        private string _fromAccount;
        private string _toAcount;
        private decimal _amount;

        private ILoggingAdapter _log = Context.GetLogger();

        public TransactionSaga(IActorRef accountsRef, string transactionId)
        {
            _accountsRef = accountsRef;

            Receive<AccountDebited>(msg =>
            {
                _log.Info("----------DEBITED---------");
                _fromAccount = msg.AccuontNumber;
                _toAcount = msg.ToAccount;
                _amount = msg.Amount;
                _transactionId = msg.TransactionId;

                var cmd = new Deposit
                {
                    Amount = msg.Amount,
                    FromAccount = msg.AccuontNumber,
                    ToAccount = msg.ToAccount,
                    TransactionId = msg.TransactionId
                };
                _accountsRef.Tell(cmd);
            });

            Receive<AccountCredited>(msg =>
            {
                _log.Info("----------CREDITED---------");
                var cmd = new CommitTransaction
                {
                    AccountNumber = msg.FromAccount,
                    TransactionId = msg.TransactionId
                };
                _accountsRef.Tell(cmd);
            });

            Receive<TransactionCommitted>(msg =>
            {
                _log.Info("----------COMMITED---------");
                _log.Info($"Transaction {msg.TransactionId} successfully finished.");
                Self.Tell(PoisonPill.Instance);
            });
        }
    }
}