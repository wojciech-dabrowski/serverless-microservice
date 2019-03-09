namespace ServerlessMicroservice.Monolith.Approach2
{
    public interface IEmailSender
    {
        void SendMail(string toMailAddress, string mailSubject, string mailBody);
    }
}