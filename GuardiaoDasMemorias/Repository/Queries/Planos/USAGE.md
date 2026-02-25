# PlanoQueries - Exemplos de Uso

## Overview
`IPlanoQueries` fornece métodos para consultar planos, limites e recursos de forma eficiente.

---

## Planos - Métodos Disponíveis

### 1. GetAllAsync()
Retorna **todos** os planos (ativos e inativos)

```csharp
var planos = await planoQueries.GetAllAsync();
// Resultado ordenado por: ordem, nome
```

### 2. GetActiveAsync()
Retorna apenas planos **ativos**

```csharp
var planosAtivos = await planoQueries.GetActiveAsync();
// WHERE ativo = true
```

### 3. GetByIdAsync(int id)
Retorna um plano específico pelo ID

```csharp
var plano = await planoQueries.GetByIdAsync(1);
if (plano != null)
{
    Console.WriteLine($"Plano: {plano.Nome} - R$ {plano.Preco}");
}
```

### 4. GetByCodeAsync(string code)
Retorna um plano pelo código (único)

```csharp
var plano = await planoQueries.GetByCodeAsync("PLANO_OURO");
// Útil para integração com gateway de pagamento
```

### 5. GetByTemaAsync(int temaId)
Retorna todos os planos de um tema

```csharp
var planosTema = await planoQueries.GetByTemaAsync(1);
// Todos os planos (ativos e inativos) do tema
```

### 6. GetByTemaActiveAsync(int temaId)
Retorna apenas planos **ativos** de um tema

```csharp
var planosTemasAtivos = await planoQueries.GetByTemaActiveAsync(1);
// Excelente para exibir na interface (escolha de planos)
```

### 7. GetByTipoPagamentoAsync(int tipoPagamentoId)
Retorna planos por tipo de pagamento

```csharp
// Tipo 1 = Pagamento Único
var planosUnicos = await planoQueries.GetByTipoPagamentoAsync(1);

// Tipo 2 = Assinatura
var planosAssinatura = await planoQueries.GetByTipoPagamentoAsync(2);
```

### 8. GetComDetalhesAsync(int id)
Retorna plano **completo** com limites e recursos

```csharp
var planoCompleto = await planoQueries.GetComDetalhesAsync(1);

if (planoCompleto != null)
{
    Console.WriteLine($"Plano: {planoCompleto.Nome}");
    Console.WriteLine($"Tema: {planoCompleto.TemaNome}");
    Console.WriteLine($"Tipo: {planoCompleto.TipoPagamentoNome}");
    
    foreach (var limite in planoCompleto.Limites)
    {
        Console.WriteLine($"  - {limite.Propriedade}: {limite.Valor}");
    }
    
    foreach (var recurso in planoCompleto.Recursos)
    {
        Console.WriteLine($"  ? {recurso.RecursoKey}: {recurso.Descricao}");
    }
}
```

---

## Limites - Métodos Disponíveis

### 1. GetLimitesAsync(int planoId)
Retorna **todos** os limites de um plano

```csharp
var limites = await planoQueries.GetLimitesAsync(1);
// Ordenado por: propriedade
```

### 2. GetLimiteByIdAsync(int id)
Retorna um limite específico

```csharp
var limite = await planoQueries.GetLimiteByIdAsync(5);
```

### 3. GetLimiteByPropriedadeAsync(int planoId, string propriedade)
Retorna um limite pela propriedade (ex: "MaxFotos")

```csharp
var maxFotos = await planoQueries.GetLimiteByPropriedadeAsync(1, "MaxFotos");

if (maxFotos != null)
{
    Console.WriteLine($"Máximo de fotos permitidas: {maxFotos.Valor}");
}
```

---

## Recursos - Métodos Disponíveis

### 1. GetRecursosAsync(int planoId)
Retorna **todos** os recursos de um plano (ativos e inativos)

```csharp
var recursosPlano = await planoQueries.GetRecursosAsync(1);
// Ordenado por: ordem, recurso_key
```

### 2. GetRecursosAtivosAsync(int planoId)
Retorna apenas recursos **ativos**

```csharp
var recursosAtivos = await planoQueries.GetRecursosAtivosAsync(1);
// Ideal para exibir na interface
```

### 3. GetRecursoByIdAsync(int id)
Retorna um recurso específico

```csharp
var recurso = await planoQueries.GetRecursoByIdAsync(3);
```

### 4. GetRecursoByKeyAsync(int planoId, string recursoKey)
Retorna um recurso pela chave

```csharp
var temQrCode = await planoQueries.GetRecursoByKeyAsync(1, "criacao_qr_code");

if (temQrCode?.Ativo == true)
{
    Console.WriteLine("? Este plano permite criar QR Codes");
}
```

---

## Casos de Uso Comuns

### Exibir Planos de um Tema na Interface
```csharp
var planos = await planoQueries.GetByTemaActiveAsync(temaId);

foreach (var plano in planos)
{
    Console.WriteLine($"[{plano.Ordem}] {plano.Nome}");
    Console.WriteLine($"  R$ {plano.Preco:N2}");
    Console.WriteLine($"  {plano.Descricao}");
}
```

### Validar Limite de Recurso
```csharp
var plano = await planoQueries.GetComDetalhesAsync(planoId);
var maxFotos = plano?.Limites.FirstOrDefault(l => l.Propriedade == "MaxFotos")?.Valor ?? 0;

if (fotosEnviadas > maxFotos)
{
    throw new Exception("Limite de fotos atingido!");
}
```

### Verificar Recursos Disponíveis
```csharp
var plano = await planoQueries.GetComDetalhesAsync(planoId);

var podeEditarTexto = plano?.Recursos.Any(r => 
    r.RecursoKey == "edicao_texto_customizado" && r.Ativo) ?? false;

var podeUsarQrCode = plano?.Recursos.Any(r => 
    r.RecursoKey == "criacao_qr_code" && r.Ativo) ?? false;

var podeUsarMusica = plano?.Recursos.Any(r => 
    r.RecursoKey == "adicionar_musica_fundo" && r.Ativo) ?? false;
```

### Buscar Plano para Pagamento
```csharp
// Integração com gateway (Stripe, Asaas, etc.)
var plano = await planoQueries.GetByCodeAsync(codigoDoPagamento);

if (plano == null)
{
    throw new Exception("Plano não encontrado!");
}

// Processar pagamento
decimal valor = plano.Preco;
```

---

## DTOs Retornados

### PlanoDto
```csharp
public class PlanoDto
{
    public int Id { get; set; }
    public int TemaId { get; set; }
    public int TipoPagamentoId { get; set; }
    public string Code { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public bool Ativo { get; set; }
    public int Ordem { get; set; }
    public DateTime Criado { get; set; }
    public DateTime? Atualizado { get; set; }
}
```

### PlanoComDetalhesDto
```csharp
public class PlanoComDetalhesDto
{
    public int Id { get; set; }
    public int TemaId { get; set; }
    public string TemaNome { get; set; }
    public int TipoPagamentoId { get; set; }
    public string TipoPagamentoNome { get; set; }
    public string Code { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public bool Ativo { get; set; }
    public int Ordem { get; set; }
    public DateTime Criado { get; set; }
    public DateTime? Atualizado { get; set; }
    public List<PlanoLimiteDto> Limites { get; set; }
    public List<PlanoRecursoDto> Recursos { get; set; }
}
```

### PlanoLimiteDto
```csharp
public class PlanoLimiteDto
{
    public int Id { get; set; }
    public int PlanoId { get; set; }
    public string Propriedade { get; set; }
    public int Valor { get; set; }
    public string Descricao { get; set; }
}
```

### PlanoRecursoDto
```csharp
public class PlanoRecursoDto
{
    public int Id { get; set; }
    public int PlanoId { get; set; }
    public string RecursoKey { get; set; }
    public string Descricao { get; set; }
    public bool Ativo { get; set; }
    public int Ordem { get; set; }
}
```

---

## Registrar no DI Container

Adicione no `Program.cs`:

```csharp
services.AddScoped<IPlanoQueries, PlanoQueries>();
```

---

## Performance

- ? Utiliza índices otimizados (ordem, tema_id+ativo, etc.)
- ? Queries otimizadas com Dapper
- ? Sem N+1 problem (GetComDetalhesAsync carrega tudo de uma vez)
- ? Ordenação eficiente
