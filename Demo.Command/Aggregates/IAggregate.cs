using Demo.Command.Commands;

namespace Demo.Command.Aggregates
{
    public interface IAggregate
    {
        void Execute(ICommandContext commandContext, ICommand command);
        void ApplyEvent(IDomainEvent domainEvent);
    }
}