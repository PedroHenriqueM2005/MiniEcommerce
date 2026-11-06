using MiniEcommerce.Console.Core;
using Xunit;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    private readonly Produto _produto = new("Headset Gamer", 250m, 10);
    private readonly CarrinhoDeCompras _carrinho = new();

    [Fact]
    public void DeveAdicionarItemComQuantidadeDentroDoEstoque()
    {
        _carrinho.AdicionarItem(_produto, 3);

        var item = Assert.Single(_carrinho.Itens);
        Assert.Equal(3, item.Quantidade);
        Assert.Equal(_produto.Id, item.Produto.Id);
    }

    [Fact]
    public void DeveAcumularQuantidadeAoAdicionarOMesmoProduto()
    {
        _carrinho.AdicionarItem(_produto, 2);
        _carrinho.AdicionarItem(_produto, 3);

        var item = Assert.Single(_carrinho.Itens);
        Assert.Equal(5, item.Quantidade);
    }

    [Fact]
    public void NaoDeveAdicionarQuantidadeSuperiorAoEstoqueDisponivel()
    {
        Assert.Throws<InvalidOperationException>(() => _carrinho.AdicionarItem(_produto, 11));
        Assert.Empty(_carrinho.Itens);
    }

    [Fact]
    public void NaoDeveAdicionarQuantidadeQueExcedaEstoqueSomandoItensExistentes()
    {
        _carrinho.AdicionarItem(_produto, 7);

        Assert.Throws<InvalidOperationException>(() => _carrinho.AdicionarItem(_produto, 4));
        Assert.Equal(7, Assert.Single(_carrinho.Itens).Quantidade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NaoDeveAdicionarItemComQuantidadeMenorOuIgualAZero(int quantidade)
    {
        Assert.Throws<ArgumentException>(() => _carrinho.AdicionarItem(_produto, quantidade));
    }

    [Fact]
    public void NaoDeveAdicionarProdutoNulo()
    {
        Assert.Throws<ArgumentNullException>(() => _carrinho.AdicionarItem(null!, 1));
    }

    [Fact]
    public void DeveCalcularSubtotalDoItem()
    {
        _carrinho.AdicionarItem(_produto, 3);

        Assert.Equal(750m, _carrinho.Itens[0].Subtotal);
    }

    [Fact]
    public void DeveCalcularTotalDoCarrinhoComVariosItens()
    {
        var mouse = new Produto("Mouse Sem Fio", 80m, 20);

        _carrinho.AdicionarItem(_produto, 2);   // 2 x 250 = 500
        _carrinho.AdicionarItem(mouse, 3);      // 3 x 80  = 240

        Assert.Equal(740m, _carrinho.Total);
    }

    [Fact]
    public void DeveRemoverItemDoCarrinho()
    {
        _carrinho.AdicionarItem(_produto, 2);

        _carrinho.RemoverItem(_produto.Id);

        Assert.Empty(_carrinho.Itens);
    }

    [Fact]
    public void NaoDeveRemoverItemQueNaoEstaNoCarrinho()
    {
        Assert.Throws<InvalidOperationException>(() => _carrinho.RemoverItem(Guid.NewGuid()));
    }
}
