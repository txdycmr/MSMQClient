using System;
using System.Runtime.Serialization;

namespace MSMQClient.Model
{
    [DataContract]
    [Serializable]
    public class MessageInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Content { get; set; }
    }
}
