namespace Demo.Command.Commands
{
    public interface ICommandContext
    {
        void Emit(IDomainEvent domainEvent);
    }
}