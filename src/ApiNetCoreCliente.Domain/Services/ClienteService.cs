using AutoMapper;
using ApiNetCoreCliente.Core.Mediator;
using ApiNetCoreCliente.Core.Messages.Notifications;
using ApiNetCoreCliente.Domain.DTOs;
using ApiNetCoreCliente.Domain.Entities;
using ApiNetCoreCliente.Domain.Interfaces.Repositories;
using ApiNetCoreCliente.Domain.Interfaces.Services;

namespace ApiNetCoreCliente.Domain.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IMapper _mapper;

    public ClienteService(IClienteRepository clienteRepository,
        IMediatorHandler mediatorHandler,
        IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mediatorHandler = mediatorHandler;
        _mapper = mapper;
    }

    public async Task<Guid> Inserir(ClienteDto cliente)
    {
        var entidadeCliente = _mapper.Map<Cliente>(cliente);

        if (await ValidarEntidade(entidadeCliente))
        {
            return await _clienteRepository.Inserir(entidadeCliente);
        }
        return Guid.Empty;
    }

    public async Task<bool> Alterar(ClienteDto cliente, Cliente entidadeCliente)
    {
        entidadeCliente.Nome = cliente.Nome;
        entidadeCliente.Idade = cliente.Idade;

        if (await ValidarEntidade(entidadeCliente))
        {
            return await _clienteRepository.Alterar(entidadeCliente);
        }
        return false;
    }

    public async Task<bool> Excluir(Cliente cliente)
    {
        return await _clienteRepository.Excluir(cliente);
    }

    public async Task<Cliente> ObterPorId(Guid id)
    {
        return await _clienteRepository.ObterPorId(id);
    }

    public async Task<ICollection<Cliente>> ObterTodos()
    {
        return await _clienteRepository.ObterTodos();
    }

    private async Task<bool> ValidarEntidade(Cliente entidadeCliente)
    {
        var resultadoValidacaoCliente = entidadeCliente.Validar();
        if (resultadoValidacaoCliente.IsValid)
            return true;

        foreach (var infoErro in resultadoValidacaoCliente.Errors.Select(s => new { Key = s.PropertyName, Value = s.ErrorMessage }))
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification(infoErro.Key, infoErro.Value));
        }
        return false;
    }
}
