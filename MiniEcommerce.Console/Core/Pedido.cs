using System.Globalization;
using System.Text;

namespace MiniEcommerce.Core;

/// <summary>
/// Pedido (compra) finalizado: consolida os itens do carrinho, realiza a baixa
/// automática no estoque e calcula o valor total da compra.
/// </summary>
public class Pedido
{
    private static readonly CultureInfo CulturaBrasileira = new("pt-BR");

    /// <summary>Identificador único do pedido.</summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>Data e hora da finalização do pedido.</summary>
    public DateTime Data { get; } = DateTime.Now;

    /// <summary>Itens consolidados do pedido.</summary>
    public IReadOnlyList<ItemPedido> Itens { get; }

    /// <summary>Valor total do pedido (soma dos subtotais).</summary>
    public decimal ValorTotal => Itens.Sum(item => item.Subtotal);

    private Pedido(IReadOnlyList<ItemPedido> itens)
    {
        Itens = itens;
    }

    /// <summary>
    /// Fecha o pedido a partir do carrinho: valida o carrinho, decrementa o estoque
    /// de cada produto adquirido e retorna o pedido consolidado.
    /// </summary>
    /// <exception cref="ArgumentNullException">Lançada quando o carrinho é nulo.</exception>
    /// <exception cref="InvalidOperationException">Lançada quando o carrinho está vazio.</exception>
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

    /// <summary>Gera o resumo financeiro formatado do pedido, com itens e valor total.</summary>
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
