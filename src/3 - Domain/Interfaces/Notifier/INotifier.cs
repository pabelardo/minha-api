using MyApiV8.Domain.Notifications;

namespace MyApiV8.Domain.Interfaces.Notifier;

public interface INotifier
{
    bool HasNotification();
    List<Notification> GetNotifications();
    IEnumerable<string> GetMessages();
    void Handle(Notification notificacao);
}
