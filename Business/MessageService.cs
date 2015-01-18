using MSMQClient.Dal;
using MSMQClient.MessagingFactory;
using MSMQClient.Model;

namespace MSMQClient.Business
{
    public class MessageService
    {
        private static readonly IMessageStrategy messageInsertStrategy = new MessageAsynchronous();
        private static readonly IMessaging.IMessage messageQueue = QueueAccess.CreateOrder();
        private static readonly IMessageDal messageDal = new MessageDal();

        public void Insert(MessageInfo messageInfo)
        {
            messageInsertStrategy.Insert(messageInfo);
        }

        public MessageInfo GetMessageInfoById(int messageId)
        {
            return messageDal.GetMessageInfo(messageId);
        }

        public MessageInfo ReceiveFromQueue(int timeout)
        {
            return messageQueue.Receive(timeout);
        }
    }
}
