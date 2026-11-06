using System.Globalization;
using MiniEcommerce.Console.Core;

CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");

var catalogo = new CatalogoProdutos();
var carrinho = new CarrinhoDeCompras();

Console.WriteLine("Bem-vindo ao MiniEcommerce!");

var encerrar = false;
while (!encerrar)
{
    ExibirMenu();
    var opcao = Console.ReadLine()?.Trim();

    if (opcao is null)
    {
        encerrar = true;
        continue;
    }

    Console.WriteLine();

    try
    {
        switch (opcao)
        {
            case "1":
                CadastrarProduto(catalogo);
                break;
            case "2":
                ListarCatalogo(catalogo);
                break;
            case "3":
                AdicionarItemAoCarrinho(catalogo, carrinho);
                break;
            case "4":
                RemoverItemDoCarrinho(carrinho);
                break;
            case "5":
                ExibirCarrinho(carrinho);
                break;
            case "6":
                carrinho = FinalizarPedido(carrinho);
                break;
            case "0":
                encerrar = true;
                Console.WriteLine("Obrigado por usar o MiniEcommerce. Até logo!");
                break;
            default:
                Console.WriteLine("Opção inválida. Por favor, escolha uma opção do menu.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"> [ERRO] {ex.Message}");
    }

    if (!encerrar)
    {
        Console.WriteLine();
        Console.Write("Pressione ENTER para voltar ao menu...");
        Console.ReadLine();
        Console.WriteLine();
    }
}

static void ExibirMenu()
{
    Console.WriteLine("==================== MINIECOMMERCE ====================");
    Console.WriteLine("1 - Cadastrar produto");
    Console.WriteLine("2 - Listar catálogo");
    Console.WriteLine("3 - Adicionar item ao carrinho");
    Console.WriteLine("4 - Remover item do carrinho");
    Console.WriteLine("5 - Exibir carrinho");
    Console.WriteLine("6 - Finalizar pedido (checkout)");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");
}

static void CadastrarProduto(CatalogoProdutos catalogo)
{
    Console.WriteLine("--- Cadastro de Produto ---");

    var nome = LerTextoObrigatorio("Nome do produto: ");
    var preco = LerDecimal("Preço do produto (ex.: 19,90): ");
    var estoque = LerInteiro("Quantidade em estoque: ");

    var produto = catalogo.Cadastrar(nome, preco, estoque);

    Console.WriteLine($"> Produto cadastrado com sucesso! ID: {produto.Id}");
}

static void ListarCatalogo(CatalogoProdutos catalogo)
{
    Console.WriteLine("--- Catálogo de Produtos ---");

    var produtos = catalogo.Listar();
    if (produtos.Count == 0)
    {
        Console.WriteLine("> Nenhum produto cadastrado ainda.");
        return;
    }

    for (var i = 0; i < produtos.Count; i++)
    {
        var produto = produtos[i];
        Console.WriteLine($"{i + 1}. {produto.Nome} | Preço: {produto.Preco:C} | Estoque: {produto.Estoque}");
    }
}

static void AdicionarItemAoCarrinho(CatalogoProdutos catalogo, CarrinhoDeCompras carrinho)
{
    Console.WriteLine("--- Adicionar Item ao Carrinho ---");

    var produtos = catalogo.Listar();
    if (produtos.Count == 0)
    {
        Console.WriteLine("> Cadastre um produto antes de adicionar itens ao carrinho.");
        return;
    }

    ListarCatalogo(catalogo);

    var numero = LerInteiro("Número do produto desejado: ");
    if (numero < 1 || numero > produtos.Count)
    {
        Console.WriteLine("> Produto inválido.");
        return;
    }

    var produto = produtos[numero - 1];
    var quantidade = LerInteiro($"Quantidade de \"{produto.Nome}\" (estoque atual: {produto.Estoque}): ");

    carrinho.AdicionarItem(produto, quantidade);

    Console.WriteLine($"> \"{produto.Nome}\" adicionado ao carrinho!");
}

static void RemoverItemDoCarrinho(CarrinhoDeCompras carrinho)
{
    Console.WriteLine("--- Remover Item do Carrinho ---");

    if (carrinho.Itens.Count == 0)
    {
        Console.WriteLine("> O carrinho está vazio.");
        return;
    }

    for (var i = 0; i < carrinho.Itens.Count; i++)
    {
        var item = carrinho.Itens[i];
        Console.WriteLine($"{i + 1}. {item.Produto.Nome} | Quantidade: {item.Quantidade} | Subtotal: {item.Subtotal:C}");
    }

    var numero = LerInteiro("Número do item a remover: ");
    if (numero < 1 || numero > carrinho.Itens.Count)
    {
        Console.WriteLine("> Item inválido.");
        return;
    }

    carrinho.RemoverItem(carrinho.Itens[numero - 1].Produto.Id);

    Console.WriteLine("> Item removido do carrinho!");
}

static void ExibirCarrinho(CarrinhoDeCompras carrinho)
{
    Console.WriteLine("--- Carrinho de Compras ---");

    if (carrinho.Itens.Count == 0)
    {
        Console.WriteLine("> O carrinho está vazio.");
        return;
    }

    foreach (var item in carrinho.Itens)
    {
        Console.WriteLine($"{item.Produto.Nome} | Quantidade: {item.Quantidade} | Subtotal: {item.Subtotal:C}");
    }

    Console.WriteLine($"TOTAL: {carrinho.Total:C}");
}

static CarrinhoDeCompras FinalizarPedido(CarrinhoDeCompras carrinho)
{
    Console.WriteLine("--- Finalizar Pedido (Checkout) ---");

    var pedido = Pedido.Finalizar(carrinho);

    Console.WriteLine("> Pedido confirmado com sucesso! Estoque atualizado.");
    Console.WriteLine();
    Console.WriteLine(pedido.GerarResumo());

    return new CarrinhoDeCompras();
}

static string LerTextoObrigatorio(string rotulo)
{
    while (true)
    {
        Console.Write(rotulo);
        var entrada = Console.ReadLine();

        if (entrada is null)
            throw new InvalidOperationException("Entrada de dados encerrada inesperadamente.");

        entrada = entrada.Trim();
        if (entrada.Length > 0)
            return entrada;

        Console.WriteLine("> O valor informado não pode ficar em branco.");
    }
}

static decimal LerDecimal(string rotulo)
{
    while (true)
    {
        var entrada = LerTextoObrigatorio(rotulo);

        if (decimal.TryParse(entrada, NumberStyles.Number, CultureInfo.CurrentCulture, out var valor) ||
            decimal.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out valor))
        {
            return valor;
        }

        Console.WriteLine("> Informe um valor monetário válido (ex.: 19,90).");
    }
}

static int LerInteiro(string rotulo)
{
    while (true)
    {
        var entrada = LerTextoObrigatorio(rotulo);

        if (int.TryParse(entrada, out var valor))
            return valor;

        Console.WriteLine("> Informe um número inteiro válido.");
    }
}
