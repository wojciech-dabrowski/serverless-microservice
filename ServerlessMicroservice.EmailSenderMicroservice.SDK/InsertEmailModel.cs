namespace ServerlessMicroservice.EmailSenderMicroservice.SDK
{
    public class InsertEmailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
