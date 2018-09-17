using System;
using Demo.Command.Commands;

namespace Demo.Bank.Commands
{
    public class CreateAccount : ICommand
    {
        public string AccountNumber { get; set; }
        public string CausationId { get; }
        public string RoutingKey => AccountNumber;
        public decimal InitialBalance { get; set; }
        public CreateAccount(string accountNumber, decimal initialBalance = 0)
        {
            CausationId = Guid.NewGuid().ToString();
            AccountNumber = accountNumber;
            InitialBalance = initialBalance;
        }
    }
}