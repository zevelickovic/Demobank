namespace Demo.Command.Commands
{
    public interface ICommand
    {
        string CausationId { get; }
        string RoutingKey { get; }
    }
}
