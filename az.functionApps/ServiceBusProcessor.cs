using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace az.functionApps
{
    public class ServiceBusProcessor
    {
        private readonly ILogger _logger;

        public ServiceBusProcessor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MessageSender>();
        }

        [Function(nameof(ServiceBusProcessor))]
        public async Task Run(
            [ServiceBusTrigger("azure-intro-topic", "Subscription1", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation($"Subscription1: {message.Body}");
            

             // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }

        // Another Subscription
        [Function("ServiceBusProcessor3")]
        public async Task Run1(
            [ServiceBusTrigger("azure-intro-topic", "Subscription3", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation($"Subscription3: {message.Body}");

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
