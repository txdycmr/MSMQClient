using MSMQClient.Model;

namespace MSMQClient.IMessaging
{
    public interface IMessage
    {
        MessageInfo Receive();

        MessageInfo Receive(int timeout);

        void Send(MessageInfo messageInfo);
    }
}
