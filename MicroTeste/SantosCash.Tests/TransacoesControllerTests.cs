using Moq;
using Microsoft.AspNetCore.Mvc;
using Controllers;
using Services;
using DTOs;
using Xunit;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantosCash.Tests;

public class TransacoesControllerTests
{
    private readonly Mock<ITransacoesService> _transacoesServiceMock;
    private readonly TransacoesController _controller;

    public TransacoesControllerTests()
    {
        _transacoesServiceMock = new Mock<ITransacoesService>();
        _controller = new TransacoesController(_transacoesServiceMock.Object);
    }

    // Testes para o método GetAllTransacoes
    [Fact]
    public async Task GetAllTransacoes_ShouldReturnOkResult_WithTransacoes()
    {
        // Arrange
        var transacoesDTO = new List<TransacoesDTO>
        {
            new TransacoesDTO { Txid = "tx1", Valor = 100 },
            new TransacoesDTO { Txid = "tx2", Valor = 200 }
        };

        _transacoesServiceMock.Setup(service => service.GetAll())
                              .ReturnsAsync(transacoesDTO);

        // Act
        var result = await _controller.GetAllTransacoes();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<TransacoesDTO>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<TransacoesDTO>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
    }

    // Testes para o método GetTransacaoByTxid
    [Fact]
    public async Task GetTransacaoByTxid_ReturnsOk_WhenTransacaoExists()
    {
        // Arrange
        var txid = "valid-txid";
        var transacaoDto = new TransacoesDTO { Txid = txid }; // Mock do retorno esperado
        _transacoesServiceMock.Setup(service => service.GetTransacoesDTOByTxidAsync(txid))
            .ReturnsAsync(transacaoDto); // Configura o mock para retornar a transação

        // Act
        var result = await _controller.GetTransacaoByTxid(txid);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica se o resultado é Ok
        var returnValue = Assert.IsType<TransacoesDTO>(okResult.Value); // Verifica se o tipo retornado é TransacoesDTO
        Assert.Equal(txid, returnValue.Txid); // Verifica se o txid é o esperado
    }

    // Teste para o caso em que a transação não existe, filtrando por txid
    [Fact]
    public async Task GetTransacaoByTxid_ReturnsNotFound_WhenTransacaoDoesNotExist()
    {
        // Arrange
        var txid = "invalid-txid";
        _transacoesServiceMock.Setup(service => service.GetTransacoesDTOByTxidAsync(txid))
            .ThrowsAsync(new KeyNotFoundException("Transação não encontrada."));

        // Act
        var result = await _controller.GetTransacaoByTxid(txid);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        
        // O retorno deve ser do tipo ErrorResponse
        var returnValue = Assert.IsType<ErrorResponse>(notFoundResult.Value);
        
        // Verifica se a mensagem de erro está correta
        Assert.Equal("Transação não encontrada.", returnValue.Message);
    }

    // Testes para o método CreateTransacao
    [Fact]
    public async Task CreateTransacao_ReturnsCreatedAtAction_WhenTransacaoIsCreated()
    {
        // Arrange
        var request = new TransacoesCreateRequestDTO
        {
            Valor = 100,
        };
        var createdTransacao = new TransacoesCreateResponseDTO
        {
            Id = 1,
            Txid = "new-txid",
            Valor = 100,
        };
        _transacoesServiceMock.Setup(service => service.CreateTransacoesDTOAsync(request))
            .ReturnsAsync(createdTransacao); // Configura o mock para retornar a transação criada

        // Act
        var result = await _controller.CreateTransacao(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result); // Verifica se o resultado é CreatedAtAction
        var returnValue = Assert.IsType<TransacoesCreateResponseDTO>(createdResult.Value); // Verifica se o tipo retornado é o esperado
        Assert.Equal("new-txid", returnValue.Txid); // Verifica se o txid da transação criada é o esperado
    }

    // Teste para o caso em que há um erro ao criar a transação
    [Fact]
    public async Task CreateTransacao_ReturnsBadRequest_WhenThereIsAnError()
    {
        // Arrange
        var request = new TransacoesCreateRequestDTO
        {
            Valor = 100,
        };
        _transacoesServiceMock.Setup(service => service.CreateTransacoesDTOAsync(request))
            .ThrowsAsync(new ArgumentException("Erro ao criar transação")); // Simula uma exceção ao tentar criar a transação

        // Act
        var result = await _controller.CreateTransacao(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); // Verifica se o resultado é BadRequest
        var returnValue = Assert.IsType<ErrorResponse>(badRequestResult.Value); // Verifica se o retorno é do tipo ErrorResponse
        Assert.Equal("Erro ao criar transação", returnValue.Message); // Verifica a mensagem de erro
    }
}
