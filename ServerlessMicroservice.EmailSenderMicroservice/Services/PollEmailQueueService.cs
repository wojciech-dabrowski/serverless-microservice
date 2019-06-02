using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace ServerlessMicroservice.EmailSenderMicroservice.Services
{
    public class PollEmailQueueService
    {
        private readonly IAmazonLambda _lambdaClient;
        private readonly IAmazonSQS _sqsClient;

        public PollEmailQueueService(IAmazonLambda lambdaClient, IAmazonSQS sqsClient)
        {
            _lambdaClient = lambdaClient;
            _sqsClient = sqsClient;
        }

        public async Task PollQueueAsync()
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
                MaxNumberOfMessages = EnvironmentVariables.MailNumberPerBatch
            };

            var sqsResult = await _sqsClient.ReceiveMessageAsync(request);

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

            var invokeResponse = await _lambdaClient.InvokeAsync(request);

            if (WasCorrectlySent(invokeResponse))
            {
                await DeleteMessage(sqsMessage, invokeResponse);
            }
        }

        private async Task DeleteMessage(Message sqsMessage, InvokeResponse invokeResponse)
        {
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = EnvironmentVariables.MailQueue,
                ReceiptHandle = sqsMessage.ReceiptHandle
            };

            await _sqsClient.DeleteMessageAsync(deleteMessageRequest);
        }

        private static bool WasCorrectlySent(InvokeResponse invokeResponse) => invokeResponse.HttpStatusCode == HttpStatusCode.OK && invokeResponse.Payload.Deserialize<bool>();
    }
}
