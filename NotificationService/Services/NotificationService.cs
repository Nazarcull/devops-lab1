using NotificationService.Models;

namespace NotificationService.Services;

public interface INotificationService
{
    IEnumerable<Notification> GetAll();
    IEnumerable<Notification> GetByRecipient(string recipient);
    Notification Send(SendNotificationDto dto);
    bool MarkAsRead(Guid id);
}

public class InMemoryNotificationService : INotificationService
{
    private readonly List<Notification> _store = new();

    public IEnumerable<Notification> GetAll()
        => _store.OrderByDescending(n => n.CreatedAt);

    public IEnumerable<Notification> GetByRecipient(string recipient)
        => _store.Where(n => n.Recipient.Equals(recipient, StringComparison.OrdinalIgnoreCase))
                 .OrderByDescending(n => n.CreatedAt);

    public Notification Send(SendNotificationDto dto)
    {
        var notification = new Notification
        {
            Recipient = dto.Recipient,
            Message = dto.Message,
            Type = dto.Type,
            CreatedAt = DateTime.UtcNow
        };
        _store.Add(notification);
        return notification;
    }

    public bool MarkAsRead(Guid id)
    {
        var notification = _store.FirstOrDefault(n => n.Id == id);
        if (notification is null) return false;
        notification.IsRead = true;
        return true;
    }
}
