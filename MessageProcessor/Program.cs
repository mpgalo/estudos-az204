using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Threading.Tasks;

namespace MessageProcessor
{
    public class Program
    {
        private const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=asyncstorfan;AccountKey=H9fob+6177tfM1aB1vYHHAtmZ5dXai9sduldxqpji0C4dVMbhAgfxPRlx/idq5VvlEMp+eA04Z/tCQRTvWWT4Q==;EndpointSuffix=core.windows.net";
        private const string queueName = "messagequeue";

        public static async Task Main(string[] args)
        {
            QueueClient client = new QueueClient(storageConnectionString, queueName);
            await client.CreateAsync();

            Console.WriteLine($"---Account Metadata---");
            Console.WriteLine($"Account Uri:\t{client.Uri}");

            Console.Write("Deseja receber? (s|n) ");
            var resp = Console.ReadLine();

            if (resp.ToLower() == "s")
            {
                Console.WriteLine($"---Existing Messages---");


                int batchSize = 10;
                TimeSpan visibilityTimeout = TimeSpan.FromSeconds(2.5d);

                Response<QueueMessage[]> messages = await client.ReceiveMessagesAsync(batchSize, visibilityTimeout);
                foreach (QueueMessage message in messages?.Value)
                {
                    Console.WriteLine($"[{message.MessageId}]\t{message.MessageText}");
                    await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                }
            }



            Console.Write("Deseja enviar? (s|n) ");
            resp = Console.ReadLine();

            if (resp.ToLower() == "s")
            {
                Console.WriteLine($"---New Messages---");
                string greeting = "Hi, Developer!";
                await client.SendMessageAsync(greeting);

                Console.WriteLine($"Sent Message:\t{greeting}");
            }


        }
    }
}
