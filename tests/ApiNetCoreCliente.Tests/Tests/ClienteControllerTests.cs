using ApiNetCoreCliente.Controllers;
using ApiNetCoreCliente.Core.Messages.Notifications;
using ApiNetCoreCliente.Domain.DTOs;
using ApiNetCoreCliente.Domain.Entities;
using ApiNetCoreCliente.Domain.Interfaces.Services;
using ApiNetCoreCliente.Tests.DataFaker;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApiNetCoreCliente.Tests.Tests;

[Trait("UnitTest - ApiNetCoreCliente", "ClienteController Tests")]
public class ClienteControllerTests
{
    private readonly AutoMocker _mocker;
    private readonly ClienteController _instance;
    public ClienteControllerTests()
    {
        _mocker = new AutoMocker();
        _instance = _mocker.CreateInstance<ClienteController>();
    }

    [Fact(DisplayName = "ObterTodos - Dados não encontrados então deve retornar erro StatusCode 404 (NotFound)")]
    public async Task ObterTodos_DadosNaoEncontrados_DeveRetornarErro404()
    {
        //Arrange
        ICollection<Cliente> clientes = null;

        _mocker.GetMock<IClienteService>()
            .Setup(s => s.ObterTodos())
            .ReturnsAsync(clientes);

        //Act
        var resultado = await _instance.ObterTodos();

        //Assert
        Assert.IsType<NotFoundResult>(resultado);
    }

    [Fact(DisplayName = "ObterTodos - Dados encontrados então deve retornar a listagem StatusCode 200 (Ok)")]
    public async Task ObterTodos_DadosEncontrados_DeveRetornarListagem()
    {
        //Arrange
        var qtdeRegistros = 12;
        var clientes = new ClienteFaker().Generate(qtdeRegistros);

        _mocker.GetMock<IClienteService>()
            .Setup(s => s.ObterTodos())
            .ReturnsAsync(clientes);

        //Act
        var resultado = await _instance.ObterTodos();
        var resultadoOkResult = resultado as ObjectResult;

        //Assert
        Assert.IsType<OkObjectResult>(resultado);
        Assert.NotNull(resultadoOkResult);
        Assert.True(((ICollection<Cliente>)resultadoOkResult.Value).Count == qtdeRegistros);
    }

    [Fact(DisplayName = "ObterPorId - Dados não encontrados então deve retornar erro StatusCode 404 (NotFound)")]
    public async Task ObterPorId_DadosNaoEncontrados_DeveRetornarErro404()
    {
        //Arrange
        Cliente cliente = null;
        Guid idCliente = Guid.Empty;

        _mocker.GetMock<IClienteService>()
            .Setup(s => s.ObterPorId(idCliente))
            .ReturnsAsync(cliente);

        //Act
        var resultado = await _instance.ObterPorId(idCliente);

        //Assert
        Assert.IsType<NotFoundResult>(resultado);
    }

    [Fact(DisplayName = "ObterPorId - Dados encontrados então deve retornar o cliente StatusCode 200 (Ok)")]
    public async Task ObterPorId_DadosEncontrados_DeveRetornarCliente()
    {
        //Arrange
        var cliente = new ClienteFaker().Generate();
        Guid idCliente = cliente.Id;

        _mocker.GetMock<IClienteService>()
            .Setup(s => s.ObterPorId(idCliente))
            .ReturnsAsync(cliente);

        //Act
        var resultado = await _instance.ObterPorId(idCliente);

        //Assert
        var resultadoRequisicao = resultado as ObjectResult;

        Assert.IsType<OkObjectResult>(resultado);
        Assert.NotNull(resultadoRequisicao);
        Assert.Equal(cliente.Nome, ((Cliente)resultadoRequisicao.Value).Nome);
    }

    [Fact(DisplayName = "Inserir - Dados inválidos deve retornar mensagem de erro e StatusCode 400 (BadRequest)")]
    public async Task Inserir_DadosInvalidos_DeveRetornarMensagemErro()
    {
        //Arrange
        var cliente = new ClienteFaker().Generate();
        cliente.Idade = 0;
        var clienteRequisicao = new ClienteDto(cliente.Nome, cliente.Idade);

        var mockDomainNotif = ObterInstanciaDomainNotificationHandler(false);
        var mockClienteService = new Mock<IClienteService>();

        var instance = new ClienteController(mockClienteService.Object, mockDomainNotif.Object);

        var resultado = await instance.Inserir(clienteRequisicao);

        //Assert
        var resultadoRequisicao = resultado as ObjectResult;

        Assert.IsType<BadRequestObjectResult>(resultado);
        Assert.NotNull(resultadoRequisicao);

        var mensagens = (IEnumerable<string>)resultadoRequisicao.Value;
        Assert.Equal("Campo Idade deve ser maior ou igual a 1.", mensagens.First());
    }

    [Fact(DisplayName = "Inserir - Dados Validos então deve inserir cliente e retornar StatusCode 200 (Ok)")]
    public async Task Inserir_DadosValidos_DeveRetornarIdentificadorCliente()
    {
        //Arrange
        var cliente = new ClienteFaker().Generate();
        var clienteRequisicao = new ClienteDto(cliente.Nome, cliente.Idade);
        Guid idCliente = cliente.Id;

        var mockDomainNotif = ObterInstanciaDomainNotificationHandler(true);
        var mockClienteService = new Mock<IClienteService>();

        mockClienteService.Setup(s => s.Inserir(clienteRequisicao))
            .ReturnsAsync(idCliente);

        var instance = new ClienteController(mockClienteService.Object, mockDomainNotif.Object);

        //Act
        var resultado = await instance.Inserir(clienteRequisicao);

        //Assert
        var resultadoRequisicao = resultado as ObjectResult;

        Assert.IsType<OkObjectResult>(resultado);
        Assert.NotNull(resultadoRequisicao);
        Assert.Equal(idCliente, resultadoRequisicao.Value);
    }

    [Fact(DisplayName = "Alterar - Cliente não encontrado então deve retornar mensagem de erro e StatusCode 404 (NotFound)")]
    public async Task Alterar_ClienteNaoEncontrado_DeveRetornarMensagemErro()
    {
        //Arrange
        var cliente = new ClienteFaker().Generate();
        var clienteRequisicao = new ClienteDto(cliente.Nome, cliente.Idade);

        //Act
        var resultado = await _instance.Alterar(Guid.NewGuid(), clienteRequisicao);

        //Assert
        var resultadoRequisicao = resultado as ObjectResult;

        Assert.IsType<NotFoundObjectResult>(resultado);
        Assert.NotNull(resultadoRequisicao);

        Assert.Equal("Cliente não encontrado", resultadoRequisicao.Value);
    }

    [Fact(DisplayName = "Alterar - Dados Invalidos então deve retornar mensagem de erro e StatusCode 400 (BadRequest)")]
    public async Task Alterar_DadosInvalidos_DeveRetornarMensagemDeErro()
    {
        //Arrange
        var cliente = new ClienteFaker().Generate();
        var clienteRequisicao = new ClienteDto(string.Empty, cliente.Idade);
        Guid idCliente = cliente.Id;

        var mockDomainNotif = ObterInstanciaDomainNotificationHandler(false);
        var mockClienteService = new Mock<IClienteService>();

        mockClienteService.Setup(s => s.ObterPorId(idCliente))
            .ReturnsAsync(cliente);

        var instance = new ClienteController(mockClienteService.Object, mockDomainNotif.Object);

        //Act
        var resultado = await instance.Alterar(idCliente, clienteRequisicao);

        //Assert
        var resultadoRequisicao = resultado as ObjectResult;

        Assert.IsType<BadRequestObjectResult>(resultado);
        Assert.NotNull(resultadoRequisicao);
        var mensagens = (IEnumerable<string>)resultadoRequisicao.Value;
        Assert.True(mensagens.Any());
    }

    [Fact(DisplayName = "Alterar - Dados Validos então deve alterar o registro e retornar StatusCode 200 (Ok)")]
    public async Task Alterar_DadosValidos_DeveExecutarComSucesso()
    {
        //Arrange
        var cliente = new ClienteFaker().Generate();
        var clienteRequisicao = new ClienteDto(cliente.Nome, cliente.Idade);
        Guid idCliente = cliente.Id;

        var mockDomainNotif = ObterInstanciaDomainNotificationHandler(true);
        var mockClienteService = new Mock<IClienteService>();

        mockClienteService.Setup(s => s.ObterPorId(idCliente))
            .ReturnsAsync(cliente);

        mockClienteService.Setup(s => s.Alterar(clienteRequisicao, cliente))
            .ReturnsAsync(true);
        
        var instance = new ClienteController(mockClienteService.Object, mockDomainNotif.Object);

        //Act
        var resultado = await instance.Alterar(idCliente, clienteRequisicao);

        //Assert
        var resultadoRequisicao = resultado as ObjectResult;

        Assert.IsType<OkObjectResult>(resultado);
        Assert.NotNull(resultadoRequisicao);
        Assert.True((bool)resultadoRequisicao.Value);
    }

    [Fact(DisplayName = "Excluir - Cliente não encontrado então deve retornar mensagem de erro e StatusCode 404 (NotFound)")]
    public async Task Excluir_ClienteNaoEncontrado_DeveRetornarMensagemErro()
    {
        //Arrange & Act
        var resultado = await _instance.Excluir(Guid.NewGuid());

        //Assert
        var resultadoRequisicao = resultado as ObjectResult;

        Assert.IsType<NotFoundObjectResult>(resultado);
        Assert.NotNull(resultadoRequisicao);

        Assert.Equal("Cliente não encontrado", resultadoRequisicao.Value);
    }

    [Fact(DisplayName = "Excluir - Cliente existente então deve excluir o registro e retornar StatusCode 200 (Ok)")]
    public async Task Excluir_ClienteExistente_DeveExcluirRegistro()
    {
        //Arrange
        var cliente = new ClienteFaker().Generate();
        Guid idCliente = cliente.Id;

        _mocker.GetMock<IClienteService>()
            .Setup(s => s.ObterPorId(idCliente))
            .ReturnsAsync(cliente);

        _mocker.GetMock<IClienteService>()
            .Setup(s => s.Excluir(cliente))
            .ReturnsAsync(true);

        //Act
        var resultado = await _instance.Excluir(idCliente);

        //Assert
        var resultadoRequisicao = resultado as ObjectResult;

        Assert.IsType<OkObjectResult>(resultado);
        Assert.NotNull(resultadoRequisicao);

        Assert.True((bool)resultadoRequisicao.Value);
    }

    private static Mock<DomainNotificationHandler> ObterInstanciaDomainNotificationHandler(bool cenarioSucesso)
    {
        var mockDomainNotif = new Mock<DomainNotificationHandler>();

        if (cenarioSucesso)
        {
            mockDomainNotif.Setup(s => s.TemNotificacao())
                .Returns(false);
        }
        else
        {
            var listaNotificacoes = new List<DomainNotification>
            {
                new DomainNotification("Idade", "Campo Idade deve ser maior ou igual a 1.")
            };

            mockDomainNotif.Setup(s => s.TemNotificacao())
                .Returns(true);

            mockDomainNotif.Setup(s => s.ObterNotificacoes())
                .Returns(listaNotificacoes);
        }

        return mockDomainNotif;
    }
}
