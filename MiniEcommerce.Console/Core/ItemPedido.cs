namespace MiniEcommerce.Console.Core;

public class ItemPedido
{
    public Produto Produto { get; }
    public int Quantidade { get; }
    public decimal PrecoUnitario { get; }

    public ItemPedido(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        Produto = produto;
        Quantidade = quantidade;
        PrecoUnitario = produto.Preco;
    }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}
