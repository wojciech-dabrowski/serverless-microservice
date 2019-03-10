using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MimeKit;
using Newtonsoft.Json;
using ServerlessMicroservice.EmailSenderMicroservice.SDK;

namespace ServerlessMicroservice.EmailSenderMicroservice.Lambdas
{
    public class SendEmailLambda
    {
        private readonly IAmazonS3 s3Client;
        private readonly IAmazonSimpleEmailService sesClient;

        public SendEmailLambda()
        {
            s3Client = new AmazonS3Client();
            sesClient = new AmazonSimpleEmailServiceClient();
        }

        public async Task<bool> SendEmailAsync(string emailTaskId, ILambdaContext context)
        {
            var insertEmailModel = await GetEmailObjectFromS3(emailTaskId);

            var sendEmailRequest = CreateRawEmailModel(insertEmailModel);

            var result = await sesClient.SendRawEmailAsync(sendEmailRequest);

            return result.HttpStatusCode == HttpStatusCode.OK;
        }

        private static SendRawEmailRequest CreateRawEmailModel(InsertEmailModel insertEmailModel)
        {
            using (var memoryStream = new MemoryStream())
            {
                var message = GetMimeMessageFromEmailModel(insertEmailModel);

                message.WriteTo(memoryStream);

                return new SendRawEmailRequest(new RawMessage(memoryStream));
            }
        }

        private static MimeMessage GetMimeMessageFromEmailModel(InsertEmailModel mailTask)
        {
            var message = new MimeMessage { Subject = mailTask.Subject };

            message.From.Add(new MailboxAddress(mailTask.From));
            message.To.Add(new MailboxAddress(String.Empty, mailTask.To));

            var body = new BodyBuilder { HtmlBody = mailTask.Body };
            message.Body = body.ToMessageBody();

            return message;
        }

        private async Task<InsertEmailModel> GetEmailObjectFromS3(string emailTaskId)
        {
            var s3Object = await s3Client.GetObjectAsync(EnvironmentVariables.MailSendingBucket, emailTaskId);

            using (var reader = new StreamReader(s3Object.ResponseStream))
            {
                var s3Contents = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<InsertEmailModel>(s3Contents);
            }
        }
    }
}
