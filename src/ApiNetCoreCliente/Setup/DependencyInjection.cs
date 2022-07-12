using MediatR;
using ApiNetCoreCliente.AutoMapper;
using ApiNetCoreCliente.Core.Mediator;
using ApiNetCoreCliente.Core.Messages.Notifications;
using ApiNetCoreCliente.Domain.Interfaces.Repositories;
using ApiNetCoreCliente.Domain.Interfaces.Services;
using ApiNetCoreCliente.Domain.Services;
using ApiNetCoreCliente.Repository;
using ApiNetCoreCliente.Repository.Repositories;

namespace ApiNetCoreCliente.Setup;
public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //MediatR
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        //Notifications
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        //AutoMapper
        services.AddAutoMapper(typeof(DtoToDomainMappingProfile));

        //Services
        services.AddScoped<IClienteService, ClienteService>();

        //Repositories
        services.AddScoped<IClienteRepository, ClienteRepository>();

        //Contexts
        services.AddScoped<ClienteContext>();
    }
}

