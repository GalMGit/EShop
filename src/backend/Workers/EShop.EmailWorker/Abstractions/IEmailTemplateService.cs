using EShop.EmailWorker.Email;

namespace EShop.EmailWorker.Abstractions;

public interface IEmailTemplateService
{
    EmailTaskDto GetRegistrationConfirmation(string toEmail, string username, string code);
    EmailTaskDto GetResetConfirmation(string toEmail, string code);
}