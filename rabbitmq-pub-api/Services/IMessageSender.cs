using System;
using rabbitmq_pub_api.Models;

namespace rabbitmq_pub_api.Services
{
    public interface IMessageSender
    {
        void SendMessage(Task task);
    }
}
