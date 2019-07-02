using System;
using NServiceBus;

namespace Events
{
    public class ApiCalledEvent : IMessage
    {
        public ApiCalledEvent(string msg)
        {
            Message = msg;
        }
        public string Message { get; set; }
    }
}
