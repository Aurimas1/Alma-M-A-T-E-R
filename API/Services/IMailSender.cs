namespace API.Services
{
    public interface IMailSender
    {
        void Send(string email, string name, string subject, string body);
    }
}
