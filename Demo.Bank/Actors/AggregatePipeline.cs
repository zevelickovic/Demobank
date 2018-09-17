using System;
using Akka.Actor;
using Akka.Persistence;
using Demo.Bank.Commands;
using Demo.Command.Aggregates;
using Demo.Command.Commands;

namespace Demo.Bank.Actors
{
    public class AggregatePipeline<T> : ReceivePersistentActor where T : IAggregate, new()
    {
        private readonly T _state;
        public override string PersistenceId { get; }

        public AggregatePipeline(string category, string aggregateId)
        {
            _state = new T();
            PersistenceId = $"{category}-{aggregateId}";
            Command<ICommand>(CommandReceived);
            Recover<IDomainEvent>(RecoverHandler);
            Recover<SnapshotOffer>(so =>
            {
                Log.Info("[recovery] SnapshotOffer");

            });
            Recover<RecoveryCompleted>(rc =>
            {
                Log.Info($"RecoveryCompleted {LastSequenceNr}");
            });

            //Command<ICommand>(CommandReceivedOnError);
            //IActorRef actor1 = Context.System.ActorOf();
            //Context.System.EventStream.Subscribe(actor1, typeof(IDomainEvent));
        }

        private bool RecoverHandler(IDomainEvent domainEvent)
        {
            _state.ApplyEvent(domainEvent);
            return true;
        }

        private bool CommandReceived(ICommand command)
        {
            var sender = Sender;
            var ctx = new CommandContext();
            try
            {
                Log.Info($"command received: {command.GetType()}");
                _state.Execute(ctx, command);
                ctx.Commit((events)=> PersistAll(events, e =>
                {
                    _state.ApplyEvent(e);
                    Context.System.EventStream.Publish(e);
                    Log.Info($"event emited{e.GetType()}");
                }));
                DeferAsync(new CommandResponse(), (msg)=>
                {
                    if(sender != ActorRefs.NoSender)
                        sender.Tell(msg, ActorRefs.NoSender);
                });
            }
            catch (Exception exception)
            {
                sender.Tell(new CommandResponse(exception, false));
                throw;
            }

            return true;
        }

        private bool CommandReceivedOnError(ICommand command)
        {
            return true;
        }
    }
}