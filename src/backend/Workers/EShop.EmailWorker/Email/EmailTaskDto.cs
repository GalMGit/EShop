namespace EShop.EmailWorker.Email;

public sealed record EmailTaskDto(
    string ToEmail, 
    string Subject, 
    string Body);