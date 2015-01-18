using MSMQClient.Business;
using MSMQClient.Model;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Transactions;

namespace MSMQClient.ViewModel
{
    public class MainViewModel
    {
        private static int transactionTimeout = 30;
        private static int queueTimeout = 20;
        private static int batchSize = 10;
        private static int threadCount = 2;
        private static int totalMessagesProcessed = 0;

        private static void ProcessMessages()
        {
            TimeSpan tsTimeout = TimeSpan.FromSeconds(Convert.ToDouble(transactionTimeout * batchSize));
            MessageService messageService = new MessageService();

            while (true)
            {
                TimeSpan datetimeStarting = new TimeSpan(DateTime.Now.Ticks);
                double elapsedTime = 0;
                int processedItems = 0;
                ArrayList queueMessages = new ArrayList();

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tsTimeout))
                {
                    for (int j = 0; j < batchSize; j++)
                    {
                        try
                        {
                            if ((elapsedTime + queueTimeout + transactionTimeout) < tsTimeout.TotalSeconds)
                            {
                                queueMessages.Add(messageService.ReceiveFromQueue(queueTimeout));
                            }
                            else
                            {
                                j = batchSize;   // exit loop
                            }
                            //update elapsed time
                            elapsedTime = new TimeSpan(DateTime.Now.Ticks).TotalSeconds - datetimeStarting.TotalSeconds;
                        }
                        catch (TimeoutException)
                        {
                            //exit loop because no more messages are waiting
                            j = batchSize;
                        }
                    }

                    for (int k = 0; k < queueMessages.Count; k++)
                    {
                        messageService.Insert((MessageInfo)queueMessages[k]);
                        processedItems++;
                        totalMessagesProcessed++;
                    }

                    //batch complete or MSMQ receive timed out
                    ts.Complete();
                }
                Debug.WriteLine("(Thread Id " + Thread.CurrentThread.ManagedThreadId + ") batch finished, " + processedItems + " items, in " + elapsedTime.ToString() + " seconds.");
            }
        }

        private static Thread[] workerThreads = new Thread[threadCount];

        public static void Start()
        {
            Thread workTicketThread;

            for (int i = 0; i < threadCount; i++)
            {

                workTicketThread = new Thread(new ThreadStart(ProcessMessages));

                // Make this a background thread, so it will terminate when the main thread/process is de-activated
                workTicketThread.IsBackground = true;
                workTicketThread.SetApartmentState(ApartmentState.STA);

                // Start the Work
                workTicketThread.Start();
                workerThreads[i] = workTicketThread;
            }

            Debug.WriteLine("Processing started...");
        }

        public static void Stop()
        {
            for (int i = 0; i < workerThreads.Length; i++)
            {
                workerThreads[i].Abort();
            }
            Debug.WriteLine(totalMessagesProcessed + " messages processed.");
        }
    }
}
