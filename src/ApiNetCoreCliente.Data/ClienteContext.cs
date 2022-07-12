using ApiNetCoreCliente.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCoreCliente.Repository;

public class ClienteContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }

    public ClienteContext(DbContextOptions<ClienteContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClienteContext).Assembly);
    }
}
