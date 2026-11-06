using System.Globalization;
using System.Text;

namespace MiniEcommerce.Console.Core;

public class Pedido
{
    private static readonly CultureInfo CulturaBrasileira = new("pt-BR");

    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Data { get; } = DateTime.Now;
    public IReadOnlyList<ItemPedido> Itens { get; }

    public decimal ValorTotal => Itens.Sum(item => item.Subtotal);

    private Pedido(IReadOnlyList<ItemPedido> itens)
    {
        Itens = itens;
    }

    public static Pedido Finalizar(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);

        if (carrinho.Itens.Count == 0)
            throw new InvalidOperationException("Não é possível finalizar um pedido com o carrinho vazio.");

        var itens = carrinho.Itens
            .Select(item => new ItemPedido(item.Produto, item.Quantidade))
            .ToList();

        foreach (var item in itens)
        {
            item.Produto.ReduzirEstoque(item.Quantidade);
        }

        return new Pedido(itens);
    }

    public string GerarResumo()
    {
        var resumo = new StringBuilder();
        resumo.AppendLine($"PEDIDO #{Id.ToString()[..8].ToUpper()} - {Data:dd/MM/yyyy HH:mm}");
        resumo.AppendLine(new string('-', 50));

        foreach (var item in Itens)
        {
            resumo.AppendLine(
                $"{item.Produto.Nome} ({item.Quantidade} x {item.PrecoUnitario.ToString("C", CulturaBrasileira)}) = {item.Subtotal.ToString("C", CulturaBrasileira)}");
        }

        resumo.AppendLine(new string('-', 50));
        resumo.Append($"TOTAL: {ValorTotal.ToString("C", CulturaBrasileira)}");

        return resumo.ToString();
    }
}
