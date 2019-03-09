namespace ServerlessMicroservice.Monolith.Approach2.EmailSender
{
    public class SendEmailModel
    {
        public SendEmailModel(string toMailAddress, string mailSubject, string mailBody)
        {
            ToMailAddress = toMailAddress;
            MailSubject = mailSubject;
            MailBody = mailBody;
        }

        public string ToMailAddress { get; }
        public string MailSubject { get; }
        public string MailBody { get; }
    }
}