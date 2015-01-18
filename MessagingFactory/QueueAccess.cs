using MSMQClient.IMessaging;
using System.Reflection;

namespace MSMQClient.MessagingFactory
{
    public sealed class QueueAccess
    {
        private QueueAccess() { }

        public static IMessage CreateOrder()
        {
            return new MSMQClient.MSMQMessaging.Message();
        }
    }
}
