namespace MailService
{
    public interface IMailProvider
    {
        void Send(string title, string to, string body);
    }
}
