using MyApiV8.Domain.Interfaces.Notifier;

namespace MyApiV8.Domain.Notifications;

public class Notifier : INotifier
{
    private readonly List<Notification> _notificacoes;

    public Notifier() => _notificacoes = [];

    public void Handle(Notification notificacao) => _notificacoes.Add(notificacao);

    public List<Notification> GetNotifications() => _notificacoes;

    public IEnumerable<string> GetMessages() => _notificacoes.Select(n => n.Message);

    public bool HasNotification() => _notificacoes.Any();
}