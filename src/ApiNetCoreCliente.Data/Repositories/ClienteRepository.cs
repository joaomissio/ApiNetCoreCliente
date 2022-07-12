using ApiNetCoreCliente.Domain.Entities;
using ApiNetCoreCliente.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCoreCliente.Repository.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClienteContext _context;

    public ClienteRepository(ClienteContext context)
    {
        _context = context;
    }

    public async Task<bool> Alterar(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        return await Commit();
    }

    public async Task<bool> Excluir(Cliente cliente)
    {
        _context.Clientes.Remove(cliente);
        return await Commit();
    }

    public async Task<Guid> Inserir(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await Commit();
        return cliente.Id;
    }

    public async Task<Cliente> ObterPorId(Guid id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<ICollection<Cliente>> ObterTodos()
    {
        return await _context.Clientes.ToListAsync();
    }

    private async Task<bool> Commit()
    {
        return await _context.SaveChangesAsync() > 0;
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
            _context?.Dispose();
        }
    }
}
