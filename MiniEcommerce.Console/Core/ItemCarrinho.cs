namespace MiniEcommerce.Console.Core;

public class ItemCarrinho
{
    public Produto Produto { get; }
    public int Quantidade { get; }

    public ItemCarrinho(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        Produto = produto;
        Quantidade = quantidade;
    }

    public decimal Subtotal => Produto.Preco * Quantidade;
}
