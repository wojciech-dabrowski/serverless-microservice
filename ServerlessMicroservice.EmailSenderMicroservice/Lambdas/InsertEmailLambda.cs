using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Newtonsoft.Json;
using ServerlessMicroservice.EmailSenderMicroservice.SDK;

namespace ServerlessMicroservice.EmailSenderMicroservice.Lambdas
{
    public class InsertEmailLambda
    {
        private readonly IAmazonS3 s3Client;
        private readonly IAmazonSQS sqsClient;

        public InsertEmailLambda()
        {
            sqsClient = new AmazonSQSClient();
            s3Client = new AmazonS3Client();
        }

        public async Task<string> InsertEmailAsync(InsertEmailModel emailModel, ILambdaContext context)
        {
            var emailStorageId = Guid.NewGuid().ToString();

            await UploadEmailToS3(emailStorageId, emailModel);

            await sqsClient.SendMessageAsync(
                EnvironmentVariables.MailQueue,
                JsonConvert.SerializeObject(emailStorageId)
            );

            return emailStorageId;
        }

        private async Task UploadEmailToS3(string emailStorageId, InsertEmailModel emailModel)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(emailModel))))
            {
                var s3PutRequest = new PutObjectRequest
                {
                    BucketName = EnvironmentVariables.MailSendingBucket,
                    Key = emailStorageId,
                    InputStream = stream
                };

                var result = await s3Client.PutObjectAsync(s3PutRequest);

                if (result.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(
                        "An error occured during putting S3 object."
                      + $"Received status code {(int) result.HttpStatusCode} in response"
                    );
                }
            }
        }
    }
}
