using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureServiceBus_QueueListener
{
    class Program
    {
        static QueueClient queueClient;
        static void Main(string[] args)
        {
            string sbConnectionString = "Endpoint=sb://mobilerecharge.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=KVb9ubc9XaV0dT/1dMj/4U21IBI=";
            string sbQueueName = "Recharge";

            try
            {
                queueClient = new QueueClient(sbConnectionString, sbQueueName);

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                queueClient.RegisterMessageHandler(ReceiveMessagesAsync, messageHandlerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
                queueClient.CloseAsync();
            }
        }

        static async Task ReceiveMessagesAsync(Message message, CancellationToken token)
        {
            try
            {
                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(message.Body)}");

                //UNCOMMENT BELOW CODE TO GENERATE EXCEPTION, SO THAT MESSSAGE WILL BE ADDED IN DEAD LETTER QUEUE
                //int i = 0;
                //i = i / Convert.ToInt32(message);

                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception ex)
            {
                await queueClient.AbandonAsync(message.SystemProperties.LockToken);
            }
        }


        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception);
            return Task.CompletedTask;
        }
    }

}
