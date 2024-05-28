using System.Net;
using Grpc.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace az.functionApps
{
    public class MultiResponse
    {
        [QueueOutput("outqueue", Connection = "AzureWebJobsStorage")]
        public string[] Messages { get; set; }
        public HttpResponseData HttpResponse { get; set; }
        
    }

    public class MessageReceiver
    {
        private readonly ILogger _logger;

        public MessageReceiver(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MessageReceiver>();
        }

        [Function("MessageReceiver")]
        public MultiResponse Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext
            )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string msgBody = new StreamReader(req.Body).ReadToEnd();
            msgBody = msgBody.Substring(1, msgBody.Length - 2);
            string msg = msgBody.Split(":")[1].TrimStart();
            _logger.LogInformation(msg);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Azure Functions Triggered Successfully");

            return new MultiResponse()
            {
                
                Messages = [msg],
                HttpResponse = response
            };
        }
    }
}


