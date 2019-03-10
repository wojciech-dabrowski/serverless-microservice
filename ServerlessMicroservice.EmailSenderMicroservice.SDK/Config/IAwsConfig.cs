namespace ServerlessMicroservice.EmailSenderMicroservice.SDK.Config
{
    public interface IAwsConfig
    {
        string AwsKey { get; }
        string AwsSecret { get; }
        string RegionName { get; }
    }
}
