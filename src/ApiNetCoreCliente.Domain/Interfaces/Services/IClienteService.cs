using ApiNetCoreCliente.Domain.DTOs;
using ApiNetCoreCliente.Domain.Entities;

namespace ApiNetCoreCliente.Domain.Interfaces.Services;

public interface IClienteService
{
    Task<ICollection<Cliente>> ObterTodos();
    Task<Cliente> ObterPorId(Guid id);
    Task<Guid> Inserir(ClienteDto cliente);
    Task<bool> Alterar(ClienteDto cliente, Cliente entidadeCliente);
    Task<bool> Excluir(Cliente cliente);
}
