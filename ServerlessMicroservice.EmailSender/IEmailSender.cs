namespace ServerlessMicroservice.EmailSender
{
    public interface IEmailSender
    {
        void SendMail(SendEmailModel model);
    }
}