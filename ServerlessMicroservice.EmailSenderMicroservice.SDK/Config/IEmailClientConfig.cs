using ServerlessMicroservice.EmailSenderMicroservice.SDK.Enum;

namespace ServerlessMicroservice.EmailSenderMicroservice.SDK.Config
{
    public interface IEmailClientConfig
    {
        IAwsConfig AwsConfig { get; }
        EnvironmentType EnvironmentType { get; }
        string FromMailAddress { get; }
    }
}
