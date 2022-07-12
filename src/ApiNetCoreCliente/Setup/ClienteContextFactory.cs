using ApiNetCoreCliente.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApiNetCoreCliente.Setup;

public class ClienteContextFactory : IDesignTimeDbContextFactory<ClienteContext>
{
    public ClienteContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .AddJsonFile("appsettings.Development.json")
           .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ClienteContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));

        return new ClienteContext(optionsBuilder.Options);
    }
}
