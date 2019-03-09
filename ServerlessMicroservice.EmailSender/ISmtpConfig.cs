namespace ServerlessMicroservice.EmailSender
{
    public interface ISmtpConfig
    {
        string SmtpServerHost { get; }
        string SmtpUserName { get; }
        string SmtpUserPassword { get; }
        int SmtpPort { get; }
    }
}