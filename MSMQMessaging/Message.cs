using MSMQClient.IMessaging;
using MSMQClient.Model;
using System;
using System.Messaging;

namespace MSMQClient.MSMQMessaging
{
    public class Message : BaseQueue, IMessage
    {
        private static readonly string queuePath = @".\Private$\ClientMessages";
        private static int queueTimeout = 20;

        public Message()
            : base(queuePath, queueTimeout)
        {
            queue.Formatter = new BinaryMessageFormatter();
        }

        public new MessageInfo Receive()
        {
            base.transactionType = MessageQueueTransactionType.Automatic;
            return (MessageInfo)((System.Messaging.Message)base.Receive()).Body;
        }

        public MessageInfo Receive(int timeout)
        {
            base.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeout));
            return Receive();
        }

        public void Send(MessageInfo messageInfo)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(messageInfo);
        }
    }
}
