using System;
using Akka.Actor;
using Demo.Bank.Aggregates;
using Demo.Command.Commands;

namespace Demo.Bank.Actors
{
    public class AccountSupervisor : ReceiveActor
    {
        public AccountSupervisor()
        {
            Receive<ICommand>(cmd =>
            {
                var child = Context.Child(cmd.RoutingKey);
                if (child.IsNobody())
                {
                    child = Context.ActorOf(Props.Create(() => new AggregatePipeline<Account>("accounts", cmd.RoutingKey)), cmd.RoutingKey);
                }

                child.Forward(cmd);
            });
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(exc =>
            {
                switch (exc)
                {
                    case NotImplementedException _:
                        return Directive.Stop;
                    default:
                        return Directive.Restart;
                }
            });
        }

        protected override void PreRestart(Exception reason, object message)
        {
            PostStop();
        }
    }
}