using MediatR;
using System.Reflection;

namespace ApiNetCoreCliente.Configurations;

public static class MediatrExtension
{
    public static void AddMediatRApi(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}
