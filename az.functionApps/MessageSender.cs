using System;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RandConsoleApp;

namespace az.functionApps
{
    public class MessageSender
    {
        private readonly ILogger _logger;

        public MessageSender(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MessageSender>();
        }

        [Function("MessageSender")]
        public void Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer)
        {
            // This message will be send in every 5 seconds
            string message = $"C# Timer trigger function executed at: {DateTime.Now}";

            HttpClient client = new HttpClient();
            HttpRequestMessage reqMsg = new(HttpMethod.Post, "http://localhost:7040/api/MessageReceiver");

            reqMsg.Content = new StringContent(JsonConvert.SerializeObject(message), 
                Encoding.UTF8, 
                "application/json");

            client.Send(reqMsg);
            // Adding a external dependency to azure function and calling it
            GFG.sampleFunc();
            _logger.LogInformation($"Message successfully sent to the queue");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
