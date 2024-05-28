using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace az.functionApps
{
    public class MessageProcessor
    {
        private readonly ILogger<MessageProcessor> _logger;

        public MessageProcessor(ILogger<MessageProcessor> logger)
        {
            _logger = logger;
        }

        [Function(nameof(MessageProcessor))]
        public void Run([QueueTrigger("outqueue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            // Send an email
            // validate something
            // alert someone
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
