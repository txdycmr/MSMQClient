using MSMQClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSMQClient.Dal
{
    public interface IMessageDal
    {
        void Insert(MessageInfo messageInfo);

        MessageInfo GetMessageInfo(int orderId);
    }
}
