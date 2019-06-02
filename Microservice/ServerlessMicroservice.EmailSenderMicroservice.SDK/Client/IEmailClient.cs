using System.Threading.Tasks;

namespace ServerlessMicroservice.EmailSenderMicroservice.SDK.Client
{
    public interface IEmailClient
    {
        Task<string> InsertEmailAsync(InsertEmailModel insertEmailModelModel);
        string InsertEmail(InsertEmailModel insertEmailModelModel);
    }
}
