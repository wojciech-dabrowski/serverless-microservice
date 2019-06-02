using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Newtonsoft.Json;
using ServerlessMicroservice.EmailSenderMicroservice.SDK.Config;
using ServerlessMicroservice.EmailSenderMicroservice.SDK.Enum;

namespace ServerlessMicroservice.EmailSenderMicroservice.SDK.Client
{
    public class EmailClient : IEmailClient, IDisposable
    {
        private readonly string insertEmailLambdaName;
        private readonly IEmailClientConfig emailClientConfig;
        private readonly Lazy<IAmazonLambda> lambdaClient;

        public EmailClient(IEmailClientConfig emailClientConfig)
        {
            this.emailClientConfig = emailClientConfig;
            insertEmailLambdaName = GetInsertEmailLambdaName(emailClientConfig.EnvironmentType);

            lambdaClient = new Lazy<IAmazonLambda>(() => new AmazonLambdaClient(CreateAwsCredentials(), CreateRegionEndpoint()));
        }

        public async Task<string> InsertEmailAsync(InsertEmailModel insertEmailModelModel)
        {
            insertEmailModelModel.From = emailClientConfig.FromMailAddress;

            var lambdaRequest = new InvokeRequest
            {
                FunctionName = insertEmailLambdaName,
                Payload = JsonConvert.SerializeObject(insertEmailModelModel)
            };

            var invokeResult = await lambdaClient.Value.InvokeAsync(lambdaRequest);

            CheckLambdaResult(invokeResult);

            return invokeResult.Payload.Deserialize<string>();
        }

        public string InsertEmail(InsertEmailModel insertEmailModelModel) => InsertEmailAsync(insertEmailModelModel).GetAwaiter().GetResult();

        private string GetInsertEmailLambdaName(EnvironmentType environmentType)
        {
            switch (environmentType)
            {
                case EnvironmentType.Test:
                    return "TestLambdaName";
                case EnvironmentType.Prod:
                    return "ProdLambdaName";
                default:
                    throw new ArgumentOutOfRangeException(nameof(environmentType), environmentType, null);
            }
        }

        private RegionEndpoint CreateRegionEndpoint()
        {
            var regionName = emailClientConfig.AwsConfig.RegionName;
            var region = RegionEndpoint.GetBySystemName(regionName);

            return region;
        }

        private BasicAWSCredentials CreateAwsCredentials()
        {
            var awsKey = emailClientConfig.AwsConfig.AwsKey;
            var awsSecret = emailClientConfig.AwsConfig.AwsSecret;
            var credentials = new BasicAWSCredentials(awsKey, awsSecret);

            return credentials;
        }

        private static void CheckLambdaResult(InvokeResponse invokeResult)
        {
            if (invokeResult.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Lambda call failed: {invokeResult.HttpStatusCode}");
            }

            if (invokeResult.FunctionError != null)
            {
                throw new Exception($"Lambda internal error: {invokeResult.FunctionError}");
            }
        }

        public void Dispose()
        {
            if (lambdaClient.IsValueCreated)
            {
                lambdaClient.Value.Dispose();
            }
        }
    }
}
