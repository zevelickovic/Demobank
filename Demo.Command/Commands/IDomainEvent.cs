namespace Demo.Command.Commands
{
    public interface IDomainEvent
    {
        string CausationId { get; }
    }
}