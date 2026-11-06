namespace MiniEcommerce.Console.Core;

public class CatalogoProdutos
{
    private readonly List<Produto> _produtos = [];

    public IReadOnlyList<Produto> Listar()
    {
        return _produtos.AsReadOnly();
    }

    public Produto Cadastrar(string nome, decimal preco, int estoque)
    {
        var nomeNormalizado = nome.Trim();
        var nomeJaCadastrado = _produtos.Any(p => p.Nome.Equals(nomeNormalizado, StringComparison.OrdinalIgnoreCase));

        if (nomeJaCadastrado)
            throw new InvalidOperationException("Já existe um produto cadastrado com este nome.");

        var produto = new Produto(nome, preco, estoque);
        _produtos.Add(produto);

        return produto;
    }

    public Produto? BuscarPorId(Guid id)
    {
        return _produtos.FirstOrDefault(p => p.Id == id);
    }
}
