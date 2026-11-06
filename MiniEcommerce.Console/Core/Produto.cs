namespace MiniEcommerce.Console.Core;

public class Produto
{
    public Guid Id { get; }
    public string Nome { get; }
    public decimal Preco { get; }
    public int Estoque { get; private set; }

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

    public bool PossuiEstoque(int quantidade)
    {
        return quantidade > 0 && Estoque >= quantidade;
    }

    public void ReduzirEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade a reduzir deve ser maior que zero.", nameof(quantidade));

        if (quantidade > Estoque)
            throw new InvalidOperationException("Estoque insuficiente para reduzir a quantidade solicitada.");

        Estoque -= quantidade;
    }
}
