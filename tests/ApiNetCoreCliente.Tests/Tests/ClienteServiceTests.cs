using AutoMapper;
using ApiNetCoreCliente.Core.Mediator;
using ApiNetCoreCliente.Core.Messages.Notifications;
using ApiNetCoreCliente.Domain.DTOs;
using ApiNetCoreCliente.Domain.Entities;
using ApiNetCoreCliente.Domain.Interfaces.Repositories;
using ApiNetCoreCliente.Domain.Services;
using ApiNetCoreCliente.Tests.DataFaker;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApiNetCoreCliente.Tests.Tests;

[Trait("UnitTest - ApiNetCoreCliente", "ClienteService Tests")]
public class ClienteServiceTests
{
    private readonly AutoMocker _mocker;
    private readonly ClienteService _instance;
    public ClienteServiceTests()
    {
        _mocker = new AutoMocker();
        _instance = _mocker.CreateInstance<ClienteService>();
    }

    [Fact(DisplayName = "ObterTodos - Dados obtidos com sucesso")]
    public async Task ObterTodos_DadosEncontrados_DeveRetornarRegistros()
    {
        //Arrange 
        var clientes = new ClienteFaker().Generate(1);

        _mocker.GetMock<IClienteRepository>()
            .Setup(s => s.ObterTodos())
            .ReturnsAsync(clientes);

        //Act
        var resultado = await _instance.ObterTodos();

        //Assert
        _mocker.GetMock<IClienteRepository>().Verify(v => v.ObterTodos(), Times.Once);
        Assert.True(resultado.Count == 1);
    }

    [Fact(DisplayName = "ObterPorId - Dados obtidos com sucesso")]
    public async Task ObterPorId_DadosEncontrados_DeveRetornarRegistro()
    {
        //Arrange 
        var cliente = new ClienteFaker().Generate();
        Guid idCliente = cliente.Id;

        _mocker.GetMock<IClienteRepository>()
            .Setup(s => s.ObterPorId(idCliente))
            .ReturnsAsync(cliente);

        //Act
        var resultado = await _instance.ObterPorId(idCliente);

        //Assert
        _mocker.GetMock<IClienteRepository>().Verify(v => v.ObterPorId(idCliente), Times.Once);
        Assert.Equal(cliente.Nome, resultado.Nome);
    }

    [Fact(DisplayName = "Inserir - Cliente cadastrado com sucesso")]
    public async Task Inserir_ClienteCadastrado_DeveRetornarIdentificador()
    {
        //Arrange 
        var cliente = new ClienteFaker().Generate();
        var clienteDto = new ClienteDto(cliente.Nome, cliente.Idade);
        Guid idCliente = cliente.Id;

        _mocker.GetMock<IClienteRepository>()
            .Setup(s => s.Inserir(cliente))
            .ReturnsAsync(idCliente);

        _mocker.GetMock<IMapper>()
            .Setup(s => s.Map<Cliente>(clienteDto))
            .Returns(cliente);

        //Act
        var resultado = await _instance.Inserir(clienteDto);

        //Assert
        _mocker.GetMock<IClienteRepository>().Verify(v => v.Inserir(cliente), Times.Once);
        Assert.Equal(idCliente, resultado);
    }

    [Fact(DisplayName = "Inserir - Cliente com dados inválidos deve gerar notificações de erro")]
    public async Task Inserir_DadosInvalidos_DeveGerarNotificacaoErro()
    {
        //Arrange 
        var cliente = new ClienteFaker().Generate();
        cliente.Nome = string.Empty;
        var clienteDto = new ClienteDto(cliente.Nome, cliente.Idade);
        Guid idCliente = Guid.Empty;

        _mocker.GetMock<IClienteRepository>()
            .Setup(s => s.Inserir(cliente))
            .ReturnsAsync(idCliente);

        _mocker.GetMock<IMapper>()
            .Setup(s => s.Map<Cliente>(clienteDto))
            .Returns(cliente);

        //Act
        var resultado = await _instance.Inserir(clienteDto);

        //Assert
        _mocker.GetMock<IClienteRepository>().Verify(v => v.Inserir(cliente), Times.Never);
        _mocker.GetMock<IMediatorHandler>().Verify(v => v.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
        Assert.Equal(idCliente, resultado);
    }

    [Fact(DisplayName = "Alterar - Cliente alterado com sucesso")]
    public async Task Alterar_ClienteAlterado_DeveRetornarIndicadorSucesso()
    {
        //Arrange 
        var cliente = new ClienteFaker().Generate();
        var clienteDto = new ClienteDto(cliente.Nome, cliente.Idade);

        _mocker.GetMock<IClienteRepository>()
            .Setup(s => s.Alterar(cliente))
            .ReturnsAsync(true);

        //Act
        var resultado = await _instance.Alterar(clienteDto, cliente);

        //Assert
        _mocker.GetMock<IClienteRepository>().Verify(v => v.Alterar(cliente), Times.Once);
        Assert.True(resultado);
    }

    [Fact(DisplayName = "Alterar - Cliente com dados inválidos deve gerar notificações de erro")]
    public async Task Alterar_DadosInvalidos_DeveGerarNotificacaoErro()
    {
        //Arrange 
        var cliente = new ClienteFaker().Generate();
        var clienteDto = new ClienteDto(string.Empty, cliente.Idade);

        _mocker.GetMock<IClienteRepository>()
            .Setup(s => s.Alterar(cliente))
            .ReturnsAsync(true);

        //Act
        var resultado = await _instance.Alterar(clienteDto, cliente);

        //Assert
        _mocker.GetMock<IClienteRepository>().Verify(v => v.Alterar(cliente), Times.Never);
        _mocker.GetMock<IMediatorHandler>().Verify(v => v.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
        Assert.False(resultado);
    }

    [Fact(DisplayName = "Excluir - Cliente excluido com sucesso")]
    public async Task Excluir_ClienteExcluido_DeveRetornarIndicadorSucesso()
    {
        //Arrange 
        var cliente = new ClienteFaker().Generate();

        _mocker.GetMock<IClienteRepository>()
            .Setup(s => s.Excluir(cliente))
            .ReturnsAsync(true);

        //Act
        var resultado = await _instance.Excluir(cliente);

        //Assert
        _mocker.GetMock<IClienteRepository>().Verify(v => v.Excluir(cliente), Times.Once);
        Assert.True(resultado);
    }
}
