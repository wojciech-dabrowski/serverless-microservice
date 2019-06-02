using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.SQS;
using ServerlessMicroservice.EmailSenderMicroservice.SDK;
using ServerlessMicroservice.EmailSenderMicroservice.Services;

namespace ServerlessMicroservice.EmailSenderMicroservice.Lambdas
{
    public class InsertEmailLambda
    {
        public async Task<string> InsertEmailAsync(InsertEmailModel emailModel, ILambdaContext context)
        {
            var s3Client = new AmazonS3Client();
            var sqsClient = new AmazonSQSClient();

            var insertEmailService = new InsertEmailService(s3Client, sqsClient);

            return await insertEmailService.InsertEmailAsync(emailModel);
        }
    }
}
