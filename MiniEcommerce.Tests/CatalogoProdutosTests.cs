using MiniEcommerce.Console.Core;
using Xunit;

namespace MiniEcommerce.Tests;

public class CatalogoProdutosTests
{
    [Fact]
    public void DeveCadastrarProdutoNoCatalogo()
    {
        var catalogo = new CatalogoProdutos();

        var produto = catalogo.Cadastrar("Cadeira Gamer", 1200m, 8);

        Assert.Contains(produto, catalogo.Listar());
        Assert.Single(catalogo.Listar());
    }

    [Fact]
    public void NaoDevePermitirCadastrarProdutosComMesmoNome()
    {
        var catalogo = new CatalogoProdutos();
        catalogo.Cadastrar("Cadeira Gamer", 1200m, 8);

        Assert.Throws<InvalidOperationException>(() => catalogo.Cadastrar("cadeira gamer", 999m, 3));
    }

    [Fact]
    public void NaoDeveAceitarProdutoComPrecoNegativoViaCatalogo()
    {
        var catalogo = new CatalogoProdutos();

        Assert.Throws<ArgumentException>(() => catalogo.Cadastrar("Cadeira Gamer", -10m, 8));
    }

    [Fact]
    public void NaoDeveAceitarProdutoComEstoqueNegativoViaCatalogo()
    {
        var catalogo = new CatalogoProdutos();

        Assert.Throws<ArgumentException>(() => catalogo.Cadastrar("Cadeira Gamer", 1200m, -5));
    }

    [Fact]
    public void DeveListarProdutosCadastrados()
    {
        var catalogo = new CatalogoProdutos();
        catalogo.Cadastrar("Produto A", 10m, 1);
        catalogo.Cadastrar("Produto B", 20m, 2);

        Assert.Equal(2, catalogo.Listar().Count);
    }

    [Fact]
    public void DeveBuscarProdutoPorId()
    {
        var catalogo = new CatalogoProdutos();
        var produto = catalogo.Cadastrar("Produto A", 10m, 1);

        Assert.Equal(produto.Id, catalogo.BuscarPorId(produto.Id)!.Id);
    }

    [Fact]
    public void DeveRetornarNuloQuandoProdutoNaoExiste()
    {
        var catalogo = new CatalogoProdutos();

        Assert.Null(catalogo.BuscarPorId(Guid.NewGuid()));
    }
}
