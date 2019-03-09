namespace ServerlessMicroservice.Monolith.Approach2.EmailSender
{
    public interface IEmailSender
    {
        void SendMail(SendEmailModel model);
    }
}