namespace MiniEcommerce.Core;

/// <summary>
/// Representa um produto do catálogo com preço e quantidade em estoque.
/// </summary>
public class Produto
{
    /// <summary>Identificador único do produto.</summary>
    public Guid Id { get; }

    /// <summary>Nome comercial do produto.</summary>
    public string Nome { get; }

    /// <summary>Preço de venda unitário (nunca negativo).</summary>
    public decimal Preco { get; }

    /// <summary>Quantidade disponível em estoque (nunca negativa).</summary>
    public int Estoque { get; private set; }

    /// <summary>
    /// Cria um produto validando nome, preço e estoque.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada quando nome, preço ou estoque são inválidos.</exception>
    public Produto(string nome, decimal preco, int estoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(nome));

        if (preco < 0)
            throw new ArgumentException("O preço do produto não pode ser negativo.", nameof(preco));

        if (estoque < 0)
            throw new ArgumentException("O estoque do produto não pode ser negativo.", nameof(estoque));

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        Preco = preco;
        Estoque = estoque;
    }

    /// <summary>Indica se há estoque suficiente para a quantidade informada.</summary>
    public bool PossuiEstoque(int quantidade)
    {
        return quantidade > 0 && Estoque >= quantidade;
    }

    /// <summary>
    /// Realiza a baixa de estoque após a venda de uma quantidade do produto.
    /// </summary>
    /// <exception cref="ArgumentException">Lançada quando a quantidade não é positiva.</exception>
    /// <exception cref="InvalidOperationException">Lançada quando não há estoque suficiente.</exception>
    public void ReduzirEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade a reduzir deve ser maior que zero.", nameof(quantidade));

        if (quantidade > Estoque)
            throw new InvalidOperationException("Estoque insuficiente para reduzir a quantidade solicitada.");

        Estoque -= quantidade;
    }
}
