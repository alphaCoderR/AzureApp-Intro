using Azure.Messaging.ServiceBus;


const string serviceBusConnString = "";
const string serviceBusQueueName = "az-namespace-queue-1";


// Creating client and processor
// The processor will be processing the tasks
ServiceBusClient client;
ServiceBusProcessor processor = default!;


async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.Out.WriteLine(body);
    await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.Out.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}

client = new ServiceBusClient(serviceBusConnString);
processor = client.CreateProcessor(serviceBusQueueName, new ServiceBusProcessorOptions());

try
{
    // Handling the messages as well as the error tasks   
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    // Starting the message receiving operation
    await processor.StartProcessingAsync();
    Console.WriteLine("Press any key to end the application");
    Console.ReadKey();

    // Processor will go on running and receiving messages from the
    // queue unless it is stopped
    Console.WriteLine("\nStopping the receiver");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped reciving messages");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
finally
{
    // Calling DisposeAsync on client types is required to ensure that network
    // resources and other unmanaged objects are properly cleaned up.
    await processor.DisposeAsync();
    await client.DisposeAsync();
}