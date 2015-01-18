using MSMQClient.IMessaging;
using MSMQClient.MessagingFactory;
using MSMQClient.Model;

namespace MSMQClient.Business
{
    public class MessageAsynchronous:IMessageStrategy
    {
        private static readonly IMessage asynchMessage = QueueAccess.CreateOrder();

        public void Insert(MessageInfo messageInfo)
        {
            asynchMessage.Send(messageInfo);
        }
    }
}
