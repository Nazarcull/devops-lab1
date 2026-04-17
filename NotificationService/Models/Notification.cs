namespace NotificationService.Models;

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Recipient { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.Info;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
}

public enum NotificationType
{
    Info,
    Warning,
    Success,
    Error
}

public class SendNotificationDto
{
    public string Recipient { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.Info;
}
