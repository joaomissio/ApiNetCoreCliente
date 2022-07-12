using ApiNetCoreCliente.Domain.Entities;

namespace ApiNetCoreCliente.Domain.Interfaces.Repositories;

public interface IClienteRepository : IDisposable
{
    Task<ICollection<Cliente>> ObterTodos();
    Task<Cliente> ObterPorId(Guid id);
    Task<Guid> Inserir(Cliente cliente);
    Task<bool> Alterar(Cliente cliente);
    Task<bool> Excluir(Cliente cliente);
}
