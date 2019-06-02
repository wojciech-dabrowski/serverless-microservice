using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Newtonsoft.Json;
using ServerlessMicroservice.EmailSenderMicroservice.SDK;

namespace ServerlessMicroservice.EmailSenderMicroservice.Services
{
    public class InsertEmailService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IAmazonSQS _sqsClient;

        public InsertEmailService(IAmazonS3 s3Client, IAmazonSQS sqsClient)
        {
            _s3Client = s3Client;
            _sqsClient = sqsClient;
        }

        public async Task<string> InsertEmailAsync(InsertEmailModel emailModel)
        {
            var emailStorageId = Guid.NewGuid().ToString();

            await UploadEmailToS3(emailStorageId, emailModel);

            await _sqsClient.SendMessageAsync(
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

                var result = await _s3Client.PutObjectAsync(s3PutRequest);

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
