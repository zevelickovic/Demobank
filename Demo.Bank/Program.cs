using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actor.Internal;
using Demo.Bank.Actors;
using Demo.Bank.Aggregates;
using Demo.Bank.Commands;
using Topshelf;

namespace Demo.Bank
{
    class Program
    {
        private ActorSystem _system;
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
            {
                x.Service<Program>(s =>
                {
                    s.ConstructUsing(name => new Program());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Sample Bank Backend");
                x.SetDisplayName("BankBackend");
                x.SetServiceName("BankBackend");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }

        private void Stop()
        {
            _system.Terminate().Wait();
        }

        private void Start()
        {
            _system = ActorSystem.Create("demobank");
            var props = Props.Create(() => new AggregatePipeline<Account>("accounts", "1"));

            var accounts = _system.ActorOf<AccountSupervisor>("accounts");
            var transactions = _system.ActorOf(Props.Create(()=> new TransactionSupervisor(accounts)) ,  "transactions");
            
            accounts.Tell(new CreateAccount("1", 100));

            
            accounts.Tell(new CreateAccount("2", 100));

            accounts.Tell(new StartTransaction(Guid.NewGuid().ToString(), "1", "2", 10, Guid.NewGuid().ToString()));

            

        }
    }
}
