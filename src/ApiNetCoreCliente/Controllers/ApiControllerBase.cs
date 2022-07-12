using MediatR;
using ApiNetCoreCliente.Core.Messages.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCoreCliente.Controllers;

public class ApiControllerBase : ControllerBase
{
    private readonly DomainNotificationHandler _notifications;
    public ApiControllerBase(INotificationHandler<DomainNotification> notifications)
    {
        _notifications = notifications as DomainNotificationHandler;
    }

    protected bool OperacaoExecutadaComSucesso() 
        => !_notifications.TemNotificacao();
    protected IEnumerable<string> ObterMensagensErro() 
        => _notifications.ObterNotificacoes().Select(c => c.Value);
}
