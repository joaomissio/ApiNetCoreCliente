using MediatR;
using ApiNetCoreCliente.Core.Messages.Notifications;
using ApiNetCoreCliente.Domain.DTOs;
using ApiNetCoreCliente.Domain.Entities;
using ApiNetCoreCliente.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiNetCoreCliente.Controllers;

[ApiController]
[Route("clientes-webapi/api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[SwaggerTag("Cadastro de Clientes")]
public class ClienteController : ApiControllerBase
{
    private readonly IClienteService _service;

    public ClienteController(IClienteService service,
        INotificationHandler<DomainNotification> notifications) : base(notifications)
    {
        _service = service;
    }

    /// <summary>
    /// Obter lista de clientes
    /// </summary>
    /// <returns></returns>
    [HttpGet("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, "Retorna a listagem de clientes", typeof(ICollection<Cliente>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhum cliente encontrado", typeof(string))]
    public async Task<IActionResult> ObterTodos()
    {
        var clientes = await _service.ObterTodos();

        if (clientes == null || clientes.Count == 0)
            return NotFound();
        else
            return Ok(clientes);
    }

    /// <summary>
    /// Obter cliente pelo identificador
    /// </summary>
    /// <param name="id">Identificador do cliente</param>
    /// <returns></returns>
    [HttpGet("[action]/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Retorna os dados do cliente", typeof(Cliente))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Cliente não encontrado", typeof(string))]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var cliente = await _service.ObterPorId(id);
        if (cliente == null)
            return NotFound();
        else
            return Ok(cliente);
    }

    /// <summary>
    /// Inserir novo cliente
    /// </summary>
    /// <param name="cliente">Dados do cliente</param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, "Retorna o id do cliente cadastrado", typeof(Guid))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados do cliente inválidos", typeof(List<string>))]
    public async Task<IActionResult> Inserir(ClienteDto cliente)
    {
        var idCliente = await _service.Inserir(cliente);

        if (OperacaoExecutadaComSucesso())
            return Ok(idCliente);
        else
            return BadRequest(ObterMensagensErro());
    }

    /// <summary>
    /// Alterar cadastro do cliente
    /// </summary>
    /// <param name="id">Identificador do cliente</param>
    /// <param name="cliente">Dados do cliente</param>
    /// <returns></returns>
    [HttpPut("[action]/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Retorna o resultado da alteração do cliente", typeof(bool))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Cliente não encontrado", typeof(string))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados do cliente inválidos", typeof(List<string>))]
    public async Task<IActionResult> Alterar(Guid id, ClienteDto cliente)
    {
        var entidadeCliente = await _service.ObterPorId(id);
        if (entidadeCliente == null)
            return NotFound("Cliente não encontrado");

        var resultadoAlteracao = await _service.Alterar(cliente, entidadeCliente);

        if (OperacaoExecutadaComSucesso())
            return Ok(resultadoAlteracao);
        else
            return BadRequest(ObterMensagensErro());
    }

    /// <summary>
    /// Excluir cliente por id
    /// </summary>
    /// <param name="id">Identificador do cliente</param>
    /// <returns></returns>
    [HttpDelete("[action]/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Retorna o resultado da exclusão do cliente", typeof(bool))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Cliente não encontrado", typeof(string))]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var dadosCliente = await _service.ObterPorId(id);
        if (dadosCliente == null)
            return NotFound("Cliente não encontrado");
        else
            return Ok(await _service.Excluir(dadosCliente));
    }
}
