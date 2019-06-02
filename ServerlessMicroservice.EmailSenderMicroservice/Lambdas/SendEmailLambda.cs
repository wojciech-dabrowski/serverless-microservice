using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.SimpleEmail;
using ServerlessMicroservice.EmailSenderMicroservice.Services;

namespace ServerlessMicroservice.EmailSenderMicroservice.Lambdas
{
    public class SendEmailLambda
    {
        public async Task<bool> SendEmailAsync(string emailTaskId, ILambdaContext context)
        {
            var s3Client = new AmazonS3Client();
            var sesClient = new AmazonSimpleEmailServiceClient();

            var sendEmailService = new SendEmailService(s3Client, sesClient);

            return await sendEmailService.SendEmailAsync(emailTaskId);
        }
    }
}
