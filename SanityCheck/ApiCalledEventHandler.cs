using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Events;
using NServiceBus;

namespace SanityCheck
{
    class ApiCalledEventHandler : IHandleMessages<ApiCalledEvent>
    {
        public Task Handle(ApiCalledEvent message, IMessageHandlerContext context)
        {
            //throw new NotImplementedException();
            var msg = message.Message;
            return Task.CompletedTask;
            
        }
    }
}
