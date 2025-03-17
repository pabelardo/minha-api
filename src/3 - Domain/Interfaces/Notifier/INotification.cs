using MyApiV8.Domain.Notifications;

namespace MyApiV8.Domain.Interfaces.Notifier;

public interface INotification
{
    Notification CreateNotification(string message);
}
