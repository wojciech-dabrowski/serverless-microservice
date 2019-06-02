using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.SQS;
using ServerlessMicroservice.EmailSenderMicroservice.Services;

namespace ServerlessMicroservice.EmailSenderMicroservice.Lambdas
{
    public class PollEmailQueueLambda
    {
        public async Task PollQueueAsync(ILambdaContext context)
        {
            var lambdaClient = new AmazonLambdaClient();
            var sqsClient = new AmazonSQSClient();

            var pollQueueService = new PollEmailQueueService(lambdaClient, sqsClient);

            await pollQueueService.PollQueueAsync();
        }
    }
}
