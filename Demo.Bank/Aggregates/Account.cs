using System;
using System.Collections.Generic;
using Demo.Bank.Commands;
using Demo.Bank.Events;
using Demo.Command.Aggregates;
using Demo.Command.Commands;

namespace Demo.Bank.Aggregates
{
    public class Account : IAggregate
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal AvailableBalance { get; set; }

        private readonly Dictionary<string, Transaction> OutstandingTransactions  = new Dictionary<string, Transaction>();

        public void Execute(ICommandContext commandContext, ICommand command)
        {
            Handle(commandContext, (dynamic)command);
        }

        private void Handle(ICommandContext commandContext, CreateAccount command)
        {
            if (!string.IsNullOrEmpty(AccountNumber))
                return;
            commandContext.Emit(new AccountCreated
            {
                AccountNumber = command.AccountNumber,
                CausationId = command.CausationId
            });
            if (command.InitialBalance > 0)
                commandContext.Emit(new AccountBalanceInitialized
                {
                    Balance = command.InitialBalance,
                    CausationId = command.CausationId,
                    AccuuntNumber = command.AccountNumber
                });

        }

        private void Handle(ICommandContext commandContext, StartTransaction command)
        {
            if (AvailableBalance <= command.Amount)
                throw new Exception("Insufficion founds");
            if(AccountNumber != command.FromAccount)
                throw new Exception("Wrong account number");
            if (command.Amount <= 0)
                return;
            if (OutstandingTransactions.ContainsKey(command.TransactionId))
                return;
            commandContext.Emit(new AccountDebited
            {
                CausationId = command.CausationId,
                Amount = command.Amount,
                ToAccount   = command.ToAccount,
                TransactionId = command.TransactionId,
                AccuontNumber = command.FromAccount
            });
        }

        private void Handle(ICommandContext commandContext, Deposit command)
        {
            if(AccountNumber != command.ToAccount)
                throw new Exception("Wrong account number");
            if (command.Amount <= 0)
                return;
            if (!OutstandingTransactions.ContainsKey(command.TransactionId))
                return;
            commandContext.Emit(new AccountCredited
            {
                AccountNumber = command.ToAccount,
                Amount = command.Amount,
                CausationId = command.CausationId,
                FromAccount = command.FromAccount,
                TransactionId = command.TransactionId
            });
        }

        private void Handle(ICommandContext commandContext, CommitTransaction command)
        {
            if (AccountNumber != command.AccountNumber)
                throw new Exception("Wrong account number");
            commandContext.Emit(
                new TransactionCommitted
                {
                    AccountNumber = command.AccountNumber,
                    CausationId = command.CausationId,
                    TransactionId = command.TransactionId
                });
        }

        public void ApplyEvent(IDomainEvent domainEvent)
        {
            Apply((dynamic)domainEvent);
        }

        private void Apply(AccountCreated accountCreated)
        {
            AccountNumber = accountCreated.AccountNumber;
        }

        private void Apply(AccountBalanceInitialized accountBalanceInitialized)
        {
            Balance = AvailableBalance = accountBalanceInitialized.Balance;
        }

        private void Apply(AccountDebited accountDebited)
        {
            AvailableBalance -= accountDebited.Amount;
            OutstandingTransactions.Add(accountDebited.TransactionId, new Transaction { Amount = accountDebited.Amount });
        }

        private void Apply(TransactionCommitted transactionCommitted)
        {
            Balance -= transactionCommitted.Amount;
            OutstandingTransactions.Remove(transactionCommitted.TransactionId);
        }

        private void Apply(AccountCredited accountCredited)
        {
            Balance += accountCredited.Amount;
            AvailableBalance += accountCredited.Amount;
        }
    }
}
