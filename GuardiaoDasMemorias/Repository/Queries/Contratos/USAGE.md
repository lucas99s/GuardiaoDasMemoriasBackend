# ContratoQueries - Exemplos de Uso

## Overview
`IContratoQueries` fornece métodos para consultar contratos de memória e seu histórico de mudanças.

---

## Contratos - Métodos Básicos

### 1. GetByIdAsync(int id)
Retorna um contrato específico pelo ID

```csharp
var contrato = await contratoQueries.GetByIdAsync(1);
if (contrato != null)
{
    Console.WriteLine($"Contrato: {contrato.Id}");
    Console.WriteLine($"Memória: {contrato.MemoriaId}");
    Console.WriteLine($"Valor: R$ {contrato.ValorPago}");
}
```

### 2. GetAllAsync()
Retorna todos os contratos

```csharp
var contratos = await contratoQueries.GetAllAsync();
// Ordenado por: criado_em DESC (mais recentes primeiro)
```

### 3. GetByClienteAsync(int clienteId)
Retorna todos os contratos de um cliente

```csharp
var contratosCli ente = await contratoQueries.GetByClienteAsync(1);
// Todos os contratos deste cliente
```

### 4. GetByMemoriaAsync(int memoriaId)
Retorna todos os contratos de uma memória

```csharp
var contratosMemoria = await contratoQueries.GetByMemoriaAsync(1);
// Histórico completo de contratos desta memória
```

### 5. GetContratoAtivoByMemoriaAsync(int memoriaId)
Retorna o contrato ATIVO de uma memória (há apenas 1!)

```csharp
var contratoAtivo = await contratoQueries.GetContratoAtivoByMemoriaAsync(1);
if (contratoAtivo != null)
{
    Console.WriteLine($"Contrato ativo: {contratoAtivo.Id}");
    // Usa índice parcial: ux_contrato_memoria_ativo_por_memoria (super rápido!)
}
```

---

## Contratos - Por Status

### Status Disponíveis:
- **1 = Pendente** - Aguardando pagamento
- **2 = Ativo** - Contrato ativo (pagamento confirmado)
- **3 = Cancelado** - Contrato foi cancelado
- **4 = Expirado** - Assinatura venceu

### Métodos:

```csharp
// Pendentes
var pendentes = await contratoQueries.GetPendentesAsync();

// Ativos
var ativos = await contratoQueries.GetAtivosAsync();

// Cancelados
var cancelados = await contratoQueries.GetCanceladosAsync();

// Expirados
var expirados = await contratoQueries.GetExpiradosAsync();

// Genérico
var porStatus = await contratoQueries.GetByStatusAsync(2); // Status = 2 (Ativo)
```

---

## Contratos - Detalhes

### 1. GetComDetalhesAsync(int id)
Retorna contrato **completo** com informações de:
- Memória
- Plano (nome + preço)
- Status (nome)
- Origem (nome)
- Cliente (nome)

```csharp
var contratoDetalhes = await contratoQueries.GetComDetalhesAsync(1);

if (contratoDetalhes != null)
{
    Console.WriteLine($"Contrato #{contratoDetalhes.Id}");
    Console.WriteLine($"Cliente: {contratoDetalhes.ClienteNome}");
    Console.WriteLine($"Plano: {contratoDetalhes.PlanoNome} - R$ {contratoDetalhes.PlanoPreco}");
    Console.WriteLine($"Status: {contratoDetalhes.StatusNome}");
    Console.WriteLine($"Origem: {contratoDetalhes.OrigemNome}");
    Console.WriteLine($"Valor pago: R$ {contratoDetalhes.ValorPago}");
    
    if (contratoDetalhes.PagoEm.HasValue)
    {
        Console.WriteLine($"Pago em: {contratoDetalhes.PagoEm:dd/MM/yyyy}");
    }
}
```

### 2. GetComDetalhesClienteAsync(int clienteId)
Retorna todos os contratos de um cliente **com detalhes**

```csharp
var contratosCliente = await contratoQueries.GetComDetalhesClienteAsync(1);

foreach (var contrato in contratosCliente)
{
    Console.WriteLine($"  [{contrato.StatusNome}] {contrato.PlanoNome} - R$ {contrato.ValorPago}");
}
```

---

## Contratos - Filtros Avançados

### 1. GetProxiamosAExpirarAsync(int diasAntes = 7)
Retorna assinaturas próximas de expirar

```csharp
// Contratos que expiram nos próximos 7 dias
var proximosExpirando = await contratoQueries.GetProxiamosAExpirarAsync(7);

// Contratos que expiram nos próximos 14 dias
var urgentes = await contratoQueries.GetProxiamosAExpirarAsync(14);

// Ideal para: enviar notificações, cobranças, etc.
```

### 2. GetByTransacaoAsync(string transacaoId)
Busca contrato por ID de transação do gateway

```csharp
// Webhook do Stripe/Asaas chegou com txn_123456
var contratos = await contratoQueries.GetByTransacaoAsync("txn_123456");
// Usa índice: ux_contrato_memoria_transacao_id_not_null (muito rápido!)
```

### 3. GetByTransacaoAsync(string transacaoId, int clienteId)
Busca segura: transação + cliente (evita acesso não autorizado)

```csharp
// Verificar se esta transação pertence a este cliente
var contrato = await contratoQueries.GetByTransacaoAsync("txn_123456", clienteLogado.Id);

if (contrato == null)
{
    throw new UnauthorizedAccessException("Transação não autorizada");
}

// Processar contrato com segurança
```

---

## Histórico de Mudanças

### 1. GetHistoricoAsync(int contratoId)
Retorna **todo** o histórico de um contrato (upgrades/downgrades)

```csharp
var historico = await contratoQueries.GetHistoricoAsync(1);

foreach (var mudanca in historico)
{
    Console.WriteLine($"[{mudanca.TipoMudanca}] {mudanca.RealizadoEm:dd/MM/yyyy}");
    if (!string.IsNullOrEmpty(mudanca.Observacao))
        Console.WriteLine($"  {mudanca.Observacao}");
}
```

### 2. GetHistoricoOrigemAsync(int contratoId)
Quando um contrato foi **substituído** por outro (upgrade)

```csharp
var upgradeDo = await contratoQueries.GetHistoricoOrigemAsync(1);

if (upgradeDo != null)
{
    Console.WriteLine($"Contrato {upgradeDo.ContratoAntigoId} foi atualizado para {upgradeDo.ContratoNovoId}");
    Console.WriteLine($"Tipo: {upgradeDo.TipoMudanca}");
}
```

### 3. GetHistoricoDestinoAsync(int contratoId)
Qual contrato foi **substituído** por este (downgrade ou renovação)

```csharp
var substituiu = await contratoQueries.GetHistoricoDestinoAsync(2);

if (substituiu != null)
{
    Console.WriteLine($"Este contrato substituiu o {substituiu.ContratoAntigoId}");
}
```

### 4. GetHistoricoByTipoAsync(string tipoMudanca)
Todos os históricos de um tipo específico

```csharp
// Todos os upgrades
var upgrades = await contratoQueries.GetHistoricoByTipoAsync("Upgrade");

// Todos os downgrades
var downgrades = await contratoQueries.GetHistoricoByTipoAsync("Downgrade");

// Todas as renovações
var renovacoes = await contratoQueries.GetHistoricoByTipoAsync("Renovação");
```

---

## Casos de Uso Comuns

### Dashboard do Cliente - Meus Contratos
```csharp
var contratoCliente = await contratoQueries.GetComDetalhesClienteAsync(usuarioLogado.ClienteId);

foreach (var contrato in contratoCliente)
{
    var statusColor = contrato.StatusNome switch
    {
        "Ativo" => "??",
        "Pendente" => "??",
        "Cancelado" => "??",
        "Expirado" => "?",
        _ => "?"
    };
    
    Console.WriteLine($"{statusColor} {contrato.PlanoNome}");
    Console.WriteLine($"   Valor: R$ {contrato.ValorPago}");
    Console.WriteLine($"   Criado: {contrato.CriadoEm:dd/MM/yyyy}");
}
```

### Webhook de Pagamento
```csharp
// Stripe/Asaas enviou confirmação
var evento = webhookPayload; // { transacao_id: "txn_123456", status: "succeeded" }

var contrato = await contratoQueries.GetByTransacaoAsync(
    evento.TransacaoId, 
    usuarioLogado.ClienteId // Segurança!
);

if (contrato != null)
{
    // Marcar como pago
    await contratoCommands.MarkAsPaidAsync(contrato.Id, evento.TransacaoId);
    Console.WriteLine("? Contrato marcado como pago!");
}
```

### Monitorar Assinaturas Vencendo
```csharp
// Job diário (Quartz, Hangfire, etc.)
var proximosAVencer = await contratoQueries.GetProxiamosAExpirarAsync(7);

foreach (var contrato in proximosAVencer)
{
    var detalhe = await contratoQueries.GetComDetalhesAsync(contrato.Id);
    
    // Enviar email de notificação
    await emailService.SendAsync(
        detalhe.ClienteNome,
        $"Sua assinatura vence em {detalhe.ExpiraEm:dd/MM/yyyy}",
        "renovar-agora.html"
    );
}
```

### Processar Upgrade de Plano
```csharp
// Cliente quer fazer upgrade
var contratoAtual = await contratoQueries.GetContratoAtivoByMemoriaAsync(memoriaId);
var novoPlano = await planoQueries.GetByIdAsync(novoPlanoId);

if (contratoAtual != null && novoPlano != null)
{
    // Criar novo contrato
    var contratoNovo = new ContratoMemoria
    {
        MemoriaId = contratoAtual.MemoriaId,
        PlanoId = novoPlano.Id,
        ClienteId = usuarioLogado.ClienteId,
        ValorPago = novoPlano.Preco,
        ContratoStatusId = 2, // Ativo
        ContratoOrigemId = 1,
        CriadoEm = DateTime.UtcNow
    };
    
    var novoContratoId = await contratoCommands.CreateAsync(contratoNovo);
    
    // Processar upgrade em transação
    await contratoCommands.ProcessUpgradeAsync(
        contratoAtual.Id,
        novoContratoId,
        "Upgrade",
        $"Upgrade de {planoAtual.Nome} para {novoPlano.Nome}"
    );
}
```

---

## DTOs Retornados

### ContratoDto
```csharp
public class ContratoDto
{
    public int Id { get; set; }
    public int MemoriaId { get; set; }
    public int PlanoId { get; set; }
    public int ContratoStatusId { get; set; }
    public int ContratoOrigemId { get; set; }
    public int ClienteId { get; set; }
    public decimal ValorPago { get; set; }
    public string? TransacaoId { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? PagoEm { get; set; }
    public DateTime? ExpiraEm { get; set; }
    public DateTime? CanceladoEm { get; set; }
}
```

### ContratoComDetalhesDto
```csharp
public class ContratoComDetalhesDto
{
    public int Id { get; set; }
    public int MemoriaId { get; set; }
    public string MemoriaNome { get; set; }
    public int PlanoId { get; set; }
    public string PlanoNome { get; set; }
    public decimal PlanoPreco { get; set; }
    public int ContratoStatusId { get; set; }
    public string StatusNome { get; set; }
    public int ContratoOrigemId { get; set; }
    public string OrigemNome { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; }
    public decimal ValorPago { get; set; }
    public string? TransacaoId { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? PagoEm { get; set; }
    public DateTime? ExpiraEm { get; set; }
    public DateTime? CanceladoEm { get; set; }
}
```

### ContratoHistoricoDto
```csharp
public class ContratoHistoricoDto
{
    public int Id { get; set; }
    public int ContratoAntigoId { get; set; }
    public int ContratoNovoId { get; set; }
    public string TipoMudanca { get; set; } // "Upgrade", "Downgrade", "Renovação"
    public string? Observacao { get; set; }
    public DateTime RealizadoEm { get; set; }
}
```

---

## Registrar no DI Container

Adicione no `Program.cs`:

```csharp
services.AddScoped<IContratoQueries, ContratoQueries>();
services.AddScoped<IContratoCommands, ContratoCommands>();
```

---

## Índices Utilizados

As queries usam os índices otimizados:

- **ux_contrato_memoria_ativo_por_memoria** ? GetContratoAtivoByMemoriaAsync (instantâneo!)
- **ux_contrato_memoria_transacao_id_not_null** ? GetByTransacaoAsync (muito rápido!)
- **IX_contrato_memoria_cliente_id** ? GetByClienteAsync
- **(contrato_status_id, expira_em)** ? GetProxiamosAExpirarAsync

---

## Performance

- ? Queries otimizadas com índices
- ? Sem N+1 problem
- ? LEFT JOIN para dados que podem não existir
- ? Paginação pronta (adicionar depois se necessário)
