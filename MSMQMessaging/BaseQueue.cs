using System;
using System.Messaging;

namespace MSMQClient.MSMQMessaging
{
    public class BaseQueue : IDisposable
    {
        protected MessageQueueTransactionType transactionType = MessageQueueTransactionType.Automatic;
        protected MessageQueue queue;
        protected TimeSpan timeout;

        public BaseQueue(string queuePath, int timeoutSeconds)
        {
            if (!MessageQueue.Exists(queuePath))
            {
                MessageQueue.Create(queuePath, true);
            }
            queue = new MessageQueue(queuePath);
            timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeoutSeconds));

            // Performance optimization since we don't need these features
            queue.DefaultPropertiesToSend.AttachSenderId = false;
            queue.DefaultPropertiesToSend.UseAuthentication = false;
            queue.DefaultPropertiesToSend.UseEncryption = false;
            queue.DefaultPropertiesToSend.AcknowledgeType = AcknowledgeTypes.None;
            queue.DefaultPropertiesToSend.UseJournalQueue = false;
        }

        public virtual object Receive()
        {
            try
            {
                using (System.Messaging.Message message = queue.Receive(timeout, transactionType))
                {
                    return message;
                }
            }
            catch (MessageQueueException mqex)
            {
                if (mqex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    throw new TimeoutException();
                }

                throw;
            }
        }

        public virtual void Send(object msg)
        {
            queue.Send(msg, transactionType);
        }

        public void Dispose()
        {
            queue.Dispose();
        }
    }
}
