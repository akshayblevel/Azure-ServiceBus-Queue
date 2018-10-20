using Microsoft.Azure.ServiceBus;
using System;
using System.Text;

namespace AzureServiceBus_Queue
{
    class Program
    {
        static QueueClient queueClient;
        static void Main(string[] args)
        {
            string sbConnectionString = "Endpoint=sb://mobilerecharge.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=KVb9ubc9XaV0dT/1dMj/4U21IBI=";
            string sbQueueName = "Recharge";

            string messageBody = string.Empty;
            try
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Mobile Recharge");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Operators");
                Console.WriteLine("1. Vodafone");
                Console.WriteLine("2. Airtel");
                Console.WriteLine("3. JIO");
                Console.WriteLine("-------------------------------------------------------");

                Console.WriteLine("Operator:");
                string mobileOperator = Console.ReadLine();
                Console.WriteLine("Amount:");
                string amount = Console.ReadLine();
                Console.WriteLine("Mobile:");
                string mobile = Console.ReadLine();

                Console.WriteLine("-------------------------------------------------------");

                switch (mobileOperator)
                {
                    case "1":
                        mobileOperator = "Vodafone";
                        break;
                    case "2":
                        mobileOperator = "Airtel";
                        break;
                    case "3":
                        mobileOperator = "JIO";
                        break;
                    default:
                        break;
                }

                messageBody = mobileOperator + "*" + mobile + "*" + amount;

                queueClient = new QueueClient(sbConnectionString, sbQueueName);

                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                Console.WriteLine($"Message Added in Queue: {messageBody}");

                //ADD MESSAGE IN THE QUEUE
                queueClient.SendAsync(message);

                //SCHEDULE MESSAGE (AFTER ONE MINUTE) TO ADD IN THE QUEUE
                //DateTimeOffset scheduleTime = DateTime.UtcNow.AddMinutes(1);
                //queueClient.ScheduleMessageAsync(message, scheduleTime);

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
    }
}
