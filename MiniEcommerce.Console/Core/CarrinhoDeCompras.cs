namespace MiniEcommerce.Core;

/// <summary>
/// Carrinho de compras do cliente: acumula itens, calcula o total
/// e valida os limites de estoque a cada adição.
/// </summary>
public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = [];

    /// <summary>Itens atualmente presentes no carrinho.</summary>
    public IReadOnlyList<ItemCarrinho> Itens => _itens.AsReadOnly();

    /// <summary>Soma dos subtotais de todos os itens do carrinho.</summary>
    public decimal Total => _itens.Sum(item => item.Subtotal);

    /// <summary>
    /// Adiciona um produto ao carrinho, acumulando quantidades do mesmo produto
    /// e validando o limite de estoque disponível.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada quando a quantidade não é positiva.</exception>
    /// <exception cref="InvalidOperationException">Lançada quando a quantidade excede o estoque disponível.</exception>
    public void AdicionarItem(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        var quantidadeJaNoCarrinho = _itens
            .Where(item => item.Produto.Id == produto.Id)
            .Sum(item => item.Quantidade);

        var quantidadeTotal = quantidadeJaNoCarrinho + quantidade;
        if (!produto.PossuiEstoque(quantidadeTotal))
            throw new InvalidOperationException(
                $"Estoque insuficiente: solicitado {quantidadeTotal}, disponível {produto.Estoque}.");

        var itemExistente = _itens.FirstOrDefault(item => item.Produto.Id == produto.Id);
        if (itemExistente is not null)
            _itens.Remove(itemExistente);

        _itens.Add(new ItemCarrinho(produto, quantidadeTotal));
    }

    /// <summary>
    /// Remove do carrinho o item correspondente ao produto informado.
    /// </summary>
    /// <exception cref="InvalidOperationException">Lançada quando o produto não está no carrinho.</exception>
    public void RemoverItem(Guid produtoId)
    {
        var item = _itens.FirstOrDefault(i => i.Produto.Id == produtoId)
            ?? throw new InvalidOperationException("Este produto não está no carrinho.");

        _itens.Remove(item);
    }
}
