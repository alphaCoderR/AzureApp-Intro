using Azure.Messaging.ServiceBus;
using System.Xml;

const string serviceBusConnString = "";
const string serviceBusQueueName = "az-namespace-queue-1";
const int maxNoofMessages = 3;

// Creating client and sender
ServiceBusClient client;
ServiceBusSender sender;

client = new ServiceBusClient(serviceBusConnString);
sender = client.CreateSender(serviceBusQueueName);

/*
 * Creating a batch to store messages so we can store multiple messages
 * and send thoose messages to the queue in Azure Service Bus at a
 * single time
 */

using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();

for (int i = 0; i < maxNoofMessages; i++)
{
    Random rnd = new Random();
    if (!batch.TryAddMessage(new ServiceBusMessage($"This is a message : {rnd.Next()}")))
    {
        Console.WriteLine($"Messaqe - {i} was not sent to the batch");
    }
}

// Sending the batch to Azure Service Bus
try
{
    // Use the producer client to send the batch of messages to the Service Bus queue
    await sender.SendMessagesAsync(batch);
    Console.WriteLine($"A batch of {maxNoofMessages} messages has been published to the queue.");
}
catch (Exception ex)
{
    Console.WriteLine("Message is not sent");
}
finally
{
    // Calling DisposeAsync on client types is required to ensure that network
    // resources and other unmanaged objects are properly cleaned up.
    await sender.DisposeAsync();
    await client.DisposeAsync();
}
