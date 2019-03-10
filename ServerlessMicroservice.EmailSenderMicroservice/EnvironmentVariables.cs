using System;

namespace ServerlessMicroservice.EmailSenderMicroservice
{
    public static class EnvironmentVariables
    {
        public static string MailSendingBucket => Environment.GetEnvironmentVariable("MailSendingBucket");
        public static string MailQueue => Environment.GetEnvironmentVariable("MailQueueUrl");
        public static int MailNumberPerSecond => Int32.Parse(Environment.GetEnvironmentVariable("MailNumberPerSecond"));
        public static string SendEmailLambdaName => Environment.GetEnvironmentVariable("SendEmailLambdaName");
    }
}
