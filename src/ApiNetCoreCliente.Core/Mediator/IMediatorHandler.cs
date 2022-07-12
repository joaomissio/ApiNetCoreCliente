using ApiNetCoreCliente.Core.Messages.Notifications;

namespace ApiNetCoreCliente.Core.Mediator;

public interface IMediatorHandler
{
    Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
}
