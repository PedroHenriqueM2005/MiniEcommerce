# MiniEcommerce

Aplicação de console em **.NET (C#)** que simula um mini e-commerce, desenvolvida para a disciplina de
**Garantia da Qualidade de Software — Gestão e Qualidade de Software**.
O projeto foi construído com abordagem **TDD** (testes escritos antes da implementação) e histórico de
commits aderente ao padrão **Conventional Commits**.

## Requisitos Funcionais

| # | Requisito | Descrição |
|---|-----------|-----------|
| 1 | Cadastro de Produtos | Nome, preço e quantidade em estoque, com validação para evitar preços negativos ou estoque menor que zero |
| 2 | Carrinho de Compras | Adicionar produtos informando a quantidade, validar estoque disponível e remover itens |
| 3 | Fechamento de Pedido (Checkout) | Baixa automática no estoque e cálculo do valor total do pedido |
| 4 | Catálogo e Consulta | Listagem de produtos com preços e estoques atuais |

## Arquitetura

Solução composta por dois projetos, com o domínio centralizado no namespace `MiniEcommerce.Core`:

```
MiniEcommerce.sln
├── MiniEcommerce.Console/          → Aplicação de console + núcleo de domínio
│   ├── Program.cs                  → Menu interativo (interface com o usuário)
│   └── Core/
│       ├── Produto.cs              → Entidade produto (validações de preço/estoque)
│       ├── CatalogoProdutos.cs     → Gerenciamento do catálogo/estoque
│       ├── ItemCarrinho.cs         → Item do carrinho (produto + quantidade)
│       ├── CarrinhoDeCompras.cs    → Regras de adição/remoção e limites de estoque
│       ├── ItemPedido.cs           → Item consolidado do pedido
│       └── Pedido.cs               → Checkout, baixa de estoque e resumo financeiro
└── MiniEcommerce.Tests/            → Testes unitários (xUnit)
    ├── ProdutoTests.cs
    ├── CatalogoProdutosTests.cs
    ├── CarrinhoDeComprasTests.cs
    └── PedidoTests.cs
```

## Principais Regras de Negócio

- Não é possível cadastrar produtos com **nome vazio**, **preço negativo** ou **estoque negativo**.
- Não é possível cadastrar produtos com **nome duplicado** no catálogo.
- Não é possível adicionar ao carrinho quantidade **maior que o estoque disponível**
  (a soma de adições do mesmo produto também é considerada).
- O **checkout** não é permitido com carrinho vazio e realiza a **baixa automática do estoque**.
- O pedido preserva o **preço unitário praticado na data da compra**.

## Como Executar

Pré-requisitos: [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior.

```bash
# Restaurar dependências e compilar
dotnet build

# Executar a aplicação interativa
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

O menu permite: cadastrar produto, listar catálogo, adicionar/remover itens do carrinho,
exibir o carrinho e finalizar o pedido (checkout).

## Como Executar os Testes

```bash
dotnet test
```

| Classe de teste | Cenários cobertos |
|-----------------|-------------------|
| `ProdutoTests` | Criação válida, nome/preço/estoque inválidos, baixa de estoque |
| `CatalogoProdutosTests` | Cadastro, nomes duplicados, listagem e consulta por id |
| `CarrinhoDeComprasTests` | Adição, acúmulo de quantidades, limites de estoque, subtotal/total, remoção |
| `PedidoTests` | Checkout, baixa de estoque, valor total, resumo financeiro, carrinhos inválidos |

## Histórico de Commits (Conventional Commits)

```text
docs: add comprehensive instructions in README.md
refactor(core): improve code readability and organize namespaces
feat(console): implement interactive menu for e-commerce store
feat(pedido): implement order processing and inventory update
test(pedido): add unit tests for checkout and stock deduction
feat(carrinho): implement shopping cart logic and stock verification
test(carrinho): add unit tests for cart addition and stock limits
feat(produto): implement product entity and validation logic
test(produto): add unit tests for product validation rules
chore: initial project structure with console and xunit apps
```

Padrões utilizados por tipo de mudança:

- `feat` — nova funcionalidade
- `fix` — correção de bug
- `test` — criação/ajuste de testes
- `refactor` — melhoria de código sem mudança de comportamento
- `docs` — documentação
- `chore` — tarefas de manutenção/estrutura inicial

## Tecnologias

- **C# 12 / .NET 8**
- **xUnit** para testes unitários
- **Git + GitHub** com fluxo de commits Conventional Commits
