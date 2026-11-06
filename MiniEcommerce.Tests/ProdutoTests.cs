using MiniEcommerce.Core;
using Xunit;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void DeveCriarProdutoComDadosValidos()
    {
        var produto = new Produto("Notebook Gamer", 4500m, 10);

        Assert.Equal("Notebook Gamer", produto.Nome);
        Assert.Equal(4500m, produto.Preco);
        Assert.Equal(10, produto.Estoque);
        Assert.NotEqual(Guid.Empty, produto.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NaoDeveCriarProdutoComNomeInvalido(string? nome)
    {
        Assert.Throws<ArgumentException>(() => new Produto(nome!, 100m, 5));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    [InlineData(-99.99)]
    public void NaoDeveCriarProdutoComPrecoNegativo(double preco)
    {
        Assert.Throws<ArgumentException>(() => new Produto("Teclado Mecânico", (decimal)preco, 5));
    }

    [Fact]
    public void DeveAceitarPrecoIgualAZero()
    {
        var produto = new Produto("Amostra Grátis", 0m, 100);

        Assert.Equal(0m, produto.Preco);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void NaoDeveCriarProdutoComEstoqueNegativo(int estoque)
    {
        Assert.Throws<ArgumentException>(() => new Produto("Mouse Gamer", 50m, estoque));
    }

    [Fact]
    public void DeveReduzirEstoqueAoRegistrarVenda()
    {
        var produto = new Produto("Monitor Full HD", 900m, 10);

        produto.ReduzirEstoque(4);

        Assert.Equal(6, produto.Estoque);
    }

    [Fact]
    public void NaoDeveReduzirEstoqueAcimaDaQuantidadeDisponivel()
    {
        var produto = new Produto("Monitor Full HD", 900m, 3);

        Assert.Throws<InvalidOperationException>(() => produto.ReduzirEstoque(4));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void NaoDeveReduzirEstoqueComQuantidadeInvalida(int quantidade)
    {
        var produto = new Produto("Monitor Full HD", 900m, 3);

        Assert.Throws<ArgumentException>(() => produto.ReduzirEstoque(quantidade));
    }
}
