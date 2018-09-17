namespace Demo.Bank.Commands
{
    public class CommandResponse
    {
        public bool Success { get; private set; }
        public object Payload { get; private set; }

        public CommandResponse(bool success = true)
        {
            Success = success;
        }

        public CommandResponse(object payload, bool success = true)
        {
            Success = success;
            Payload = payload;
        }
    }
}