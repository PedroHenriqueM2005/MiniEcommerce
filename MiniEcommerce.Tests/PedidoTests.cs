using MiniEcommerce.Core;
using Xunit;

namespace MiniEcommerce.Tests;

public class PedidoTests
{
    private readonly Produto _produto = new("Teclado Mecânico", 300m, 10);
    private readonly Produto _outroProduto = new("Monitor Full HD", 900m, 5);
    private readonly CarrinhoDeCompras _carrinho = new();

    private CarrinhoDeCompras CriarCarrinhoComItens()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(_produto, 2);       // 2 x 300 = 600
        carrinho.AdicionarItem(_outroProduto, 1);  // 1 x 900 = 900

        return carrinho;
    }

    [Fact]
    public void DeveFinalizarPedidoComItensDoCarrinho()
    {
        var pedido = Pedido.Finalizar(CriarCarrinhoComItens());

        Assert.Equal(2, pedido.Itens.Count);
        Assert.NotEqual(Guid.Empty, pedido.Id);
    }

    [Fact]
    public void DeveBaixarEstoqueAposConfirmarCompra()
    {
        var carrinho = CriarCarrinhoComItens();

        Pedido.Finalizar(carrinho);

        Assert.Equal(8, _produto.Estoque);
        Assert.Equal(4, _outroProduto.Estoque);
    }

    [Fact]
    public void DeveCalcularValorTotalDoPedido()
    {
        var pedido = Pedido.Finalizar(CriarCarrinhoComItens());

        Assert.Equal(1500m, pedido.ValorTotal);
    }

    [Fact]
    public void DevePreservarPrecoUnitarioNaDataDaCompra()
    {
        var pedido = Pedido.Finalizar(CriarCarrinhoComItens());

        Assert.All(pedido.Itens, item => Assert.Equal(item.Produto.Preco, item.PrecoUnitario));
    }

    [Fact]
    public void NaoDeveFinalizarPedidoComCarrinhoVazio()
    {
        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(new CarrinhoDeCompras()));
    }

    [Fact]
    public void NaoDeveFinalizarPedidoComCarrinhoNulo()
    {
        Assert.Throws<ArgumentNullException>(() => Pedido.Finalizar(null!));
    }

    [Fact]
    public void DeveGerarResumoFinanceiroComItensETotal()
    {
        var pedido = Pedido.Finalizar(CriarCarrinhoComItens());

        var resumo = pedido.GerarResumo();

        Assert.Contains("Teclado Mecânico", resumo);
        Assert.Contains("Monitor Full HD", resumo);
        Assert.Contains("TOTAL:", resumo);
    }
}
