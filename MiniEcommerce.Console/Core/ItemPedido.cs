namespace MiniEcommerce.Core;

/// <summary>
/// Item consolidado de um pedido finalizado, preservando o preço praticado na data da compra.
/// </summary>
public class ItemPedido
{
    /// <summary>Produto adquirido.</summary>
    public Produto Produto { get; }

    /// <summary>Quantidade adquirida.</summary>
    public int Quantidade { get; }

    /// <summary>Preço unitário registrado no momento da compra.</summary>
    public decimal PrecoUnitario { get; }

    /// <summary>Cria um item de pedido a partir de um produto e uma quantidade.</summary>
    public ItemPedido(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        Produto = produto;
        Quantidade = quantidade;
        PrecoUnitario = produto.Preco;
    }

    /// <summary>Valor parcial do item (preço unitário multiplicado pela quantidade).</summary>
    public decimal Subtotal => PrecoUnitario * Quantidade;
}
