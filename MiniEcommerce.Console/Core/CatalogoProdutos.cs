namespace MiniEcommerce.Core;

/// <summary>
/// Gerencia o catálogo de produtos disponíveis na loja (cadastro, listagem e consulta).
/// </summary>
public class CatalogoProdutos
{
    private readonly List<Produto> _produtos = [];

    /// <summary>Lista imutável de todos os produtos cadastrados.</summary>
    public IReadOnlyList<Produto> Listar()
    {
        return _produtos.AsReadOnly();
    }

    /// <summary>
    /// Cadastra um novo produto no catálogo, validando os dados e nomes duplicados.
    /// </summary>
    /// <exception cref="InvalidOperationException">Lançada quando já existe produto com o mesmo nome.</exception>
    /// <exception cref="ArgumentException">Lançada quando os dados do produto são inválidos.</exception>
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

    /// <summary>Busca um produto pelo identificador, retornando nulo quando não encontrado.</summary>
    public Produto? BuscarPorId(Guid id)
    {
        return _produtos.FirstOrDefault(p => p.Id == id);
    }
}
