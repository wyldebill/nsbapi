using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events;
using NServiceBus;

namespace api2
{
    public class ApiCalledEventHandler : IHandleMessages<ApiCalledEvent>
    {
        public Task Handle(ApiCalledEvent message, IMessageHandlerContext context)
        {
            var data = message.Message;
            return Task.CompletedTask;
        }
    }
}
