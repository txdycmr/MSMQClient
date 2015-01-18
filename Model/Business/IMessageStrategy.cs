using MSMQClient.Model;

namespace MSMQClient.Business
{
    public interface IMessageStrategy
    {
        void Insert(MessageInfo messageInfo);
    }
}
