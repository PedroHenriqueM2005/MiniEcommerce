namespace MiniEcommerce.Console.Core;

public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = [];

    public IReadOnlyList<ItemCarrinho> Itens => _itens.AsReadOnly();

    public decimal Total => _itens.Sum(item => item.Subtotal);

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

    public void RemoverItem(Guid produtoId)
    {
        var item = _itens.FirstOrDefault(i => i.Produto.Id == produtoId)
            ?? throw new InvalidOperationException("Este produto não está no carrinho.");

        _itens.Remove(item);
    }
}
