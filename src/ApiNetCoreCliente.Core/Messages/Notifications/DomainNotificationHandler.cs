using MediatR;

namespace ApiNetCoreCliente.Core.Messages.Notifications;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>, IDisposable
{
    private List<DomainNotification> _notifications;

    public DomainNotificationHandler()
    {
        _notifications = new List<DomainNotification>();
    }

    public Task Handle(DomainNotification message, CancellationToken cancellationToken)
    {
        _notifications.Add(message);
        return Task.CompletedTask;
    }

    public virtual List<DomainNotification> ObterNotificacoes()
    {
        return _notifications;
    }

    public virtual bool TemNotificacao()
    {
        return ObterNotificacoes().Any();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _notifications = new List<DomainNotification>();
        }
    }
}
