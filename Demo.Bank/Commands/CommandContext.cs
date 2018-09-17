using System;
using System.Collections.Generic;
using Demo.Command.Commands;

namespace Demo.Bank.Commands
{
    public class CommandContext : ICommandContext
    {
        private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public void Emit(IDomainEvent domainEvent)
        {
            if (domainEvent != null)
                _domainEvents.Add(domainEvent);
        }

        public void Commit(Action<IEnumerable<IDomainEvent>> callBackAction)
        {
            callBackAction?.DynamicInvoke(_domainEvents);
            _domainEvents = new List<IDomainEvent>();
        }
    }
}
