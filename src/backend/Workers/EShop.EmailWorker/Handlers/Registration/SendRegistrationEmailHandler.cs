using EShop.Contracts.Identity.Events;
using EShop.EmailWorker.Abstractions;

namespace EShop.EmailWorker.Handlers.Registration;

public sealed class SendRegistrationEmailHandler(
    IEmailService emailService,
    IEmailTemplateService templateService,
    ILogger<SendRegistrationEmailHandler> logger)
{
    public async Task Handle(
        UserStartRegistrationEvent message,
        CancellationToken ct)
    {
        var email = templateService.GetRegistrationConfirmation(
            message.Email,
            message.Username,
            message.ConfirmationCode);

        var result = await emailService.SendMailAsync(
            email.ToEmail,
            email.Subject,
            email.Body);

        if (!result)
        {
            logger.LogError(
                "Failed to send registration email to {Email}",
                message.Email);

            throw new InvalidOperationException(
                $"Failed to send email to {message.Email}");
        }

        logger.LogInformation(
            "Registration email sent to {Email}",
            message.Email);
    }
}