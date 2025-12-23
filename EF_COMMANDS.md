# Guardião das Memórias - Entity Framework

## ?? Pacotes Instalados

- Microsoft.EntityFrameworkCore (9.0.0)
- Npgsql.EntityFrameworkCore.PostgreSQL (9.0.0)
- Microsoft.EntityFrameworkCore.Tools (9.0.0)

## ??? Configuração do Banco de Dados

O projeto está configurado para usar PostgreSQL hospedado no Railway com a seguinte connection string:

```
Host=shuttle.proxy.rlwy.net;Port=57050;Database=railway;Username=postgres;Password=LrjrCTOPhvAOChgilMdYZpcaTqiRxlWW;SSL Mode=Prefer;Trust Server Certificate=true
```

## ?? Comandos do Entity Framework

### 1. Criar a primeira migration
```bash
dotnet ef migrations add InitialCreate --project GuardiaoDasMemorias
```

### 2. Aplicar as migrations no banco de dados
```bash
dotnet ef database update --project GuardiaoDasMemorias
```

### 3. Adicionar uma nova migration (após alterar as entidades)
```bash
dotnet ef migrations add NomeDaMigration --project GuardiaoDasMemorias
```

### 4. Remover a última migration (se ainda não foi aplicada)
```bash
dotnet ef migrations remove --project GuardiaoDasMemorias
```

### 5. Ver o script SQL de uma migration
```bash
dotnet ef migrations script --project GuardiaoDasMemorias
```

### 6. Listar todas as migrations
```bash
dotnet ef migrations list --project GuardiaoDasMemorias
```

### 7. Reverter para uma migration específica
```bash
dotnet ef database update NomeDaMigration --project GuardiaoDasMemorias
```

### 8. Deletar o banco de dados
```bash
dotnet ef database drop --project GuardiaoDasMemorias
```

## ?? Estrutura do Banco de Dados

### PostgreSQL - Schemas e Tabelas

O banco está organizado em **schemas separados** para melhor organização:

#### **Schema: cliente**
- **cliente.clientes**: Gerenciamento de clientes
  - Colunas: id, nome, telefone, email

#### **Schema: tema**
- **tema.temas**: Temas disponíveis para templates
  - Colunas: id, nome

#### **Schema: musica**
- **musica.musicas**: Músicas vinculadas aos clientes
  - Colunas: id, nome, caminho, cliente_id
  - FK: cliente_id ? cliente.clientes.id

#### **Schema: template**
- **template.templates**: Templates vinculados a temas e clientes
  - Colunas: id, tema_id, template_id, cliente_id
  - FK: cliente_id ? cliente.clientes.id
  - FK: tema_id ? tema.temas.id

### Relacionamentos
- **musica.musicas.cliente_id** ? **cliente.clientes.id** (CASCADE DELETE)
- **template.templates.cliente_id** ? **cliente.clientes.id** (CASCADE DELETE)
- **template.templates.tema_id** ? **tema.temas.id** (RESTRICT DELETE)

### Identity Columns
Todas as chaves primárias usam `IDENTITY ALWAYS` do PostgreSQL para geração automática de IDs.

## ?? Dados Iniciais (Seed)

O banco de dados será criado com dados iniciais para:
- 2 Clientes (no schema cliente)
- 4 Temas (no schema tema)
- 3 Músicas (no schema musica)
- 3 Templates (no schema template)

## ?? Próximos Passos

1. Execute a migration inicial:
   ```bash
   dotnet ef migrations add InitialCreate --project GuardiaoDasMemorias
   dotnet ef database update --project GuardiaoDasMemorias
   ```

2. Rode o projeto:
   ```bash
   dotnet run --project GuardiaoDasMemorias
   ```

3. Acesse o Swagger para testar os endpoints

## ?? Observações Importantes

### Vantagens da Organização por Schemas

1. **Separação Lógica**: Cada módulo tem seu próprio schema
2. **Permissões Granulares**: Pode-se dar permissões específicas por schema
3. **Melhor Organização**: Facilita manutenção e escalabilidade
4. **Consultas Claras**: Queries explicitam o contexto (ex: `SELECT * FROM cliente.clientes`)

### Convenções PostgreSQL

1. **Nomenclatura**: snake_case para tabelas e colunas
2. **Schemas**: Organização modular do banco
3. **Identity**: `IDENTITY ALWAYS` para auto-incremento
4. **Case-sensitivity**: Lowercase por padrão
5. **Foreign Keys**: Suporte completo com CASCADE/RESTRICT

Todas essas práticas estão implementadas no projeto! ?

## ?? Exemplo de Consulta SQL

```sql
-- Buscar todos os clientes
SELECT * FROM cliente.clientes;

-- Buscar músicas de um cliente específico
SELECT m.* 
FROM musica.musicas m
INNER JOIN cliente.clientes c ON m.cliente_id = c.id
WHERE c.id = 1;

-- Buscar templates por tema
SELECT t.* 
FROM template.templates t
INNER JOIN tema.temas tm ON t.tema_id = tm.id
WHERE tm.nome = 'Romântico';
