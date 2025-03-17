using MyApiV8.Domain.Interfaces.Notifier;

namespace MyApiV8.Domain.Notifications;

public class Notification : INotification
{
    public string Message { get; }

    public Notification() { }

    public Notification(string mensagem)
    {
        Message = mensagem;
    }

    public Notification CreateNotification(string message) => new(message);
}