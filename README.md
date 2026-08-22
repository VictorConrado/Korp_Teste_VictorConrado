# Korp — Sistema de Faturamento e Estoque

Sistema de faturamento integrado a um serviço de estoque, desenvolvido como uma aplicação distribuída utilizando .NET e Angular.

O sistema permite cadastrar e gerenciar produtos, criar notas fiscais com múltiplos itens e realizar a impressão das notas mediante validação e baixa de estoque.

## Visão Geral

O projeto é dividido em dois serviços principais:

- **Korp.Faturamento:** responsável pelo gerenciamento das notas fiscais.
- **Korp.Estoque:** responsável pelo cadastro de produtos e controle de saldo em estoque.

O frontend foi desenvolvido em Angular e se comunica diretamente com a API de faturamento.

### Fluxo Principal

```text
Angular
   │
   ▼
Korp.Faturamento
   │
   ├── Criação da Nota Fiscal
   ├── Consulta de Produtos
   ├── Impressão da Nota
   │
   ▼
Korp.Estoque
   │
   └── Baixa de Estoque
```

---

## Funcionalidades

### Produtos
* Listagem de produtos
* Consulta de produto por ID
* Cadastro, edição e exclusão de produtos
* Controle de saldo em estoque
* Validação de código duplicado

### Notas Fiscais
* Criação de nota fiscal com inclusão de múltiplos produtos
* Cálculo do valor total
* Listagem e impressão de nota fiscal
* Controle de status da nota (Aberta, Processando, Fechada)
* Indicador de processamento durante a impressão
* Tratamento de erros

### Integração entre Serviços
O serviço de faturamento realiza comunicação com o serviço de estoque para:
1. Consultar produtos.
2. Validar disponibilidade de estoque.
3. Realizar baixa de estoque durante a impressão.

> **Nota:** Quando o serviço de estoque está indisponível, a API de faturamento retorna uma resposta adequada para o frontend.

### Concorrência e Idempotência
* **Concorrência:** Foi implementado controle de concorrência (utilizando `RowVersion` e Entity Framework Core) para evitar que duas operações simultâneas consumam o mesmo saldo disponível no estoque.
* **Idempotência:** A operação de baixa de estoque possui suporte à idempotência através de chaves registradas no banco de dados. Operações repetidas com a mesma chave não realizam uma nova baixa.

---

## Arquitetura e Estrutura

O projeto utiliza uma separação clara entre as responsabilidades de domínio, aplicação, infraestrutura e apresentação.

**Korp.Faturamento** (Regras de notas fiscais e integração com estoque)
```text
Korp.Faturamento
│
├── Controllers
├── Dominios
├── DTOs
├── Excecoes
├── Integracoes
├── Middleware
├── Servicos
└── Dados
```

**Korp.Estoque** (Regras de produtos, saldo, concorrência e idempotência)
```text
Korp.Estoque
│
├── Controllers
├── Dominios
├── DTOs
├── Excecoes
├── Servicos
└── Dados
```

---

## Como Executar

### Pré-requisitos
* .NET SDK
* Node.js e Angular CLI
* SQL Server

### Executando o Backend
Abra dois terminais e execute os serviços separadamente:

**Serviço de Faturamento:**
```bash
cd Korp.Faturamento
dotnet restore
dotnet run
```

**Serviço de Estoque:**
```bash
cd Korp.Estoque
dotnet restore
dotnet run
```
*A documentação da API (Swagger) ficará disponível em `http://localhost:<porta>/swagger`.*

### Executando o Frontend
Em um novo terminal:
```bash
cd Korp.Faturamento.Frontend
npm install
ng serve
```
*A aplicação ficará disponível em `http://localhost:4200`.*

---

## Tecnologias e Decisões de Implementação (Requisitos Técnicos)

Esta seção detalha as decisões técnicas adotadas no desenvolvimento da aplicação, respondendo aos requisitos de arquitetura solicitados:

### 1. Quais ciclos de vida do Angular foram utilizados?
Foi utilizado principalmente o ciclo de vida **`OnInit` (`ngOnInit`)** nos componentes que precisam executar alguma ação durante a inicialização da tela.
Nas telas de produtos e notas fiscais, o `ngOnInit()` é utilizado para carregar os dados necessários assim que o componente é renderizado na tela:

```typescript
ngOnInit(): void {
  this.carregarProdutos(); // ou this.carregarNotas();
}
```

### 2. Foi feito uso da biblioteca RxJS? Como?
**Sim.** O projeto utiliza RxJS fortemente através dos `Observables` retornados pelo `HttpClient` do Angular. 
As chamadas aos serviços são consumidas utilizando o método `subscribe()`, tratando os fluxos de sucesso e erro através dos callbacks `next` e `error`. Ele também foi responsável por controlar os estados da interface (como carregamento e exibição de mensagens).

```typescript
this.produtoServico.listar().subscribe({
  next: produtos => {
    this.produtos = produtos;
    this.carregandoProdutos = false;
  },
  error: erro => {
    console.error('Erro:', erro);
    this.mensagemErro = 'Não foi possível carregar os produtos.';
    this.carregandoProdutos = false;
  }
});
```

### 3. Quais outras bibliotecas foram utilizadas e para qual finalidade?
**No Frontend:**
* **Angular Common:** Utilização de diretivas e pipes comuns (`CommonModule`, `CurrencyPipe`, `DatePipe`).
* **Angular Forms:** Gerenciamento dos dados de formulários (`FormsModule` e `[(ngModel)]`).
* **Angular Router:** Navegação entre páginas (`RouterLink` e `Router`).

**No Backend:**
* **System.Net.Http.Json:** Para facilitar a comunicação HTTP e desserialização entre os microsserviços.
* **Swagger/OpenAPI:** Para geração da documentação interativa e testes dos endpoints da API.

### 4. Para componentes visuais, quais bibliotecas foram utilizadas?
**Nenhuma biblioteca externa** (como Angular Material ou Bootstrap) foi utilizada. Os componentes visuais foram desenvolvidos puramente com **HTML e CSS** nativos integrados aos recursos do Angular, garantindo uma interface simples, leve e totalmente personalizada para o projeto.

### 5. Gerenciamento de dependências no Golang
**Não se aplica.** O projeto não utiliza a linguagem Go. Todo o backend foi construído em C#/.NET.

### 6. Quais frameworks foram utilizados no Golang ou C#?
Para o desenvolvimento do backend em **C#**, foram utilizados:
* **ASP.NET Core:** Framework principal para a construção e roteamento das APIs REST.
* **Entity Framework Core (EF Core):** Framework de ORM (Object-Relational Mapping) utilizado para modelagem, persistência e acesso aos dados no SQL Server.

### 7. Como foram tratados os erros e exceções no backend?
Foi implementado um **Middleware Centralizado de Tratamento de Exceções** (`TratamentoExcecoesMiddleware`). Ele intercepta exceções na pipeline de requisições e as converte em respostas HTTP padronizadas (JSON) para o frontend.
Cenários tratados incluem:
* Nota fiscal/Produto não encontrado → `404 Not Found`
* Nota já impressa / Conflito de concorrência → `409 Conflict`
* Nota sem itens / Dados inválidos → `400 Bad Request`
* Falha de comunicação com microsserviço → Mensagem tratada no payload
* Erros inesperados → `500 Internal Server Error`

```csharp
// Exemplo de interceptação no Middleware
catch (Exception excecao)
{
    await TratarAsync(contexto, excecao);
}
```

### 8. Foi utilizado LINQ no C#? De que forma?
**Sim.** O LINQ (Language Integrated Query) foi amplamente utilizado para construir consultas tipadas ao banco de dados em conjunto com o Entity Framework Core e para manipulação de coleções em memória.

**Exemplos de uso no projeto:**
* **Busca específica (`FirstOrDefaultAsync`):** Localiza um registro único por ID.
* **Verificação de existência (`AnyAsync`):** Usado para validar se um código de produto já existe, evitando duplicações antes de inserir.
* **Projeção e Ordenação (`Select` e `OrderBy`):** Usados para mapear entidades de domínio diretamente para DTOs (Data Transfer Objects) na própria query do banco, otimizando a consulta.

```csharp
// Exemplo de projeção e ordenação com LINQ
return await _contexto.Produtos
    .AsNoTracking()
    .OrderBy(x => x.Id)
    .Select(x => new ProdutoResposta
    {
        Id = x.Id,
        Codigo = x.Codigo,
        Descricao = x.Descricao,
        Saldo = x.Saldo
    })
    .ToListAsync();
```
