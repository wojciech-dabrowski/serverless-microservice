using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace ServerlessMicroservice.EmailSenderMicroservice.Lambdas
{
    public class PollEmailQueueLambda
    {
        private readonly IAmazonLambda lambdaClient;
        private readonly IAmazonSQS sqsClient;

        public PollEmailQueueLambda()
        {
            sqsClient = new AmazonSQSClient();
            lambdaClient = new AmazonLambdaClient();
        }

        public async Task PollQueueAsync(ILambdaContext context)
        {
            var lambdaSw = Stopwatch.StartNew();
            var areMessagesToProcess = true;

            while (lambdaSw.Elapsed < TimeSpan.FromSeconds(59.0) && areMessagesToProcess)
            {
                var singleBatchSw = Stopwatch.StartNew();

                areMessagesToProcess = await ProcessSingleBatch() != 0;

                var timeLeftToNextBatch = (int) (TimeSpan.FromSeconds(1).Milliseconds - singleBatchSw.ElapsedMilliseconds);

                if (timeLeftToNextBatch > 0)
                {
                    Thread.Sleep(timeLeftToNextBatch);
                }
            }
        }

        private async Task<int> ProcessSingleBatch()
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = EnvironmentVariables.MailQueue,
                MaxNumberOfMessages = EnvironmentVariables.MailNumberPerSecond
            };

            var sqsResult = await sqsClient.ReceiveMessageAsync(request);

            var sendEmailTasks = sqsResult.Messages.Select(async sqsMessage => await InvokeLambdaAndDeleteMessage(sqsMessage));

            await Task.WhenAll(sendEmailTasks);

            return sqsResult.Messages.Count;
        }

        private async Task InvokeLambdaAndDeleteMessage(Message sqsMessage)
        {
            var request = new InvokeRequest
            {
                FunctionName = EnvironmentVariables.SendEmailLambdaName,
                Payload = sqsMessage.Body
            };

            var invokeResponse = await lambdaClient.InvokeAsync(request);

            if (!WasCorrectlySent(invokeResponse))
            {
                return;
            }

            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = EnvironmentVariables.MailQueue,
                ReceiptHandle = sqsMessage.ReceiptHandle
            };

            await sqsClient.DeleteMessageAsync(deleteMessageRequest);
        }

        private static bool WasCorrectlySent(InvokeResponse invokeResponse) => invokeResponse.HttpStatusCode == HttpStatusCode.OK && invokeResponse.Payload.Deserialize<bool>();
    }
}
