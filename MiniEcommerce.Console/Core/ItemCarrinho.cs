namespace MiniEcommerce.Core;

/// <summary>
/// Representa um item do carrinho de compras, ligando um produto à quantidade desejada.
/// </summary>
public class ItemCarrinho
{
    /// <summary>Produto adicionado ao carrinho.</summary>
    public Produto Produto { get; }

    /// <summary>Quantidade desejada do produto (sempre maior que zero).</summary>
    public int Quantidade { get; }

    /// <summary>Cria um item de carrinho validando produto e quantidade.</summary>
    /// <exception cref="ArgumentException">Lançada quando a quantidade não é positiva.</exception>
    public ItemCarrinho(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        Produto = produto;
        Quantidade = quantidade;
    }

    /// <summary>Valor parcial do item (preço unitário multiplicado pela quantidade).</summary>
    public decimal Subtotal => Produto.Preco * Quantidade;
}
