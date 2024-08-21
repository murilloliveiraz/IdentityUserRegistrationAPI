using APIThatSendAnEmailToUpdatePassword.Helpers;

namespace APIThatSendAnEmailToUpdatePassword.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest emailRequest);
    }
}
