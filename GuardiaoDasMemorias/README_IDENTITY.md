# Autenticação com ASP.NET Core Identity e JWT

## Configuração Implementada

Este projeto utiliza **ASP.NET Core Identity** para gerenciamento de usuários e **JWT (JSON Web Tokens)** para autenticação stateless.

## Estrutura de Arquivos

### Models
- **ApplicationUser.cs**: Classe que estende `IdentityUser` com propriedades personalizadas
  - `CreatedAt`: Data de criação da conta

### DTOs de Autenticação
- **RegisterDto.cs**: DTO para registro de novos usuários
- **LoginDto.cs**: DTO para login
- **AuthResponseDto.cs**: DTO de resposta com o token JWT

### Services
- **TokenService.cs**: Serviço para geração de tokens JWT

### Controllers
- **AuthController.cs**: Controller com endpoints de autenticação

## Endpoints da API

### 1. Registrar Usuário
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "password": "SenhaForte123",
  "phoneNumber": "+5511999999999"
}
```

**Resposta de Sucesso (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "usuario@exemplo.com",
  "userId": "abc123...",
  "userName": "usuario@exemplo.com",
  "expiresAt": "2024-01-01T12:00:00Z"
}
```

### 2. Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "password": "SenhaForte123"
}
```

**Resposta de Sucesso (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "usuario@exemplo.com",
  "userId": "abc123...",
  "userName": "usuario@exemplo.com",
  "expiresAt": "2024-01-01T12:00:00Z"
}
```

### 3. Obter Usuário Atual
```http
GET /api/auth/me
Authorization: Bearer {token}
```

**Resposta de Sucesso (200):**
```json
{
  "userId": "abc123...",
  "email": "usuario@exemplo.com",
  "userName": "usuario@exemplo.com",
  "phoneNumber": "+5511999999999",
  "createdAt": "2024-01-01T10:00:00Z"
}
```

### 4. Logout
```http
POST /api/auth/logout
Authorization: Bearer {token}
```

## Configurações JWT

As configurações do JWT estão em `appsettings.json`:

```json
{
  "JwtSettings": {
    "Secret": "sua-chave-secreta-muito-segura",
    "Issuer": "GuardiaoDasMemorias",
    "Audience": "GuardiaoDasMemoriasUsers",
    "ExpirationMinutes": "60"
  }
}
```

### ?? IMPORTANTE - Segurança em Produção
- **NUNCA** commite a chave secreta (`Secret`) no repositório
- Use variáveis de ambiente ou Azure Key Vault em produção
- A chave deve ter pelo menos 32 caracteres
- Exemplo de configuração por variável de ambiente:

```bash
export JwtSettings__Secret="sua-chave-super-secreta-em-producao"
```

## Regras de Senha (Configuradas no Identity)

- Mínimo de 6 caracteres
- Pelo menos 1 dígito
- Pelo menos 1 letra minúscula
- Pelo menos 1 letra maiúscula
- Caracteres especiais: não obrigatórios

## Schema do Banco de Dados

As tabelas do Identity são criadas no schema `auth`:

- `auth.users` - Usuários
- `auth.roles` - Perfis/Roles
- `auth.user_roles` - Relacionamento Usuário-Perfil
- `auth.user_claims` - Claims dos usuários
- `auth.user_logins` - Logins externos (Google, Facebook, etc)
- `auth.role_claims` - Claims dos perfis
- `auth.user_tokens` - Tokens de recuperação/confirmação

## Migrações

### Criar uma nova migration
```bash
dotnet ef migrations add AddIdentityTables
```

### Aplicar migrations ao banco de dados
```bash
dotnet ef database update
```

## Usando Autenticação em Controllers

Para proteger um endpoint, adicione o atributo `[Authorize]`:

```csharp
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class MinhaController : ControllerBase
{
    [Authorize] // Requer autenticação
    [HttpGet]
    public IActionResult Get()
    {
        // Obter ID do usuário autenticado
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        return Ok(new { message = "Usuário autenticado!", userId });
    }
    
    [Authorize(Roles = "Admin")] // Requer perfil específico
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Apenas Admin pode executar
        return Ok();
    }
}
```

## Testando no Swagger

O Swagger foi configurado para suportar autenticação JWT:

1. Faça login via `/api/auth/login` ou `/api/auth/register`
2. Copie o token retornado
3. Clique no botão "Authorize" no topo do Swagger
4. Digite: `Bearer {seu-token-aqui}`
5. Clique em "Authorize"
6. Agora você pode chamar endpoints protegidos

## Cliente HTTP (Frontend)

### Exemplo com Fetch API
```javascript
// Login
const response = await fetch('http://localhost:5000/api/auth/login', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    email: 'usuario@exemplo.com',
    password: 'SenhaForte123'
  })
});

const data = await response.json();
const token = data.token;

// Salvar token
localStorage.setItem('token', token);

// Usar token em requisições subsequentes
const protectedResponse = await fetch('http://localhost:5000/api/auth/me', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});
```

### Exemplo com Axios
```javascript
import axios from 'axios';

// Login
const { data } = await axios.post('/api/auth/login', {
  email: 'usuario@exemplo.com',
  password: 'SenhaForte123'
});

// Configurar token globalmente
axios.defaults.headers.common['Authorization'] = `Bearer ${data.token}`;

// Fazer requisições autenticadas
const user = await axios.get('/api/auth/me');
```

## Campos do ApplicationUser

O modelo `ApplicationUser` herda todos os campos do `IdentityUser` e adiciona:

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | string | ID único do usuário (herdado) |
| `UserName` | string | Nome de usuário (geralmente o email) |
| `Email` | string | Email do usuário |
| `EmailConfirmed` | bool | Se o email foi confirmado |
| `PhoneNumber` | string? | Telefone do usuário |
| `PhoneNumberConfirmed` | bool | Se o telefone foi confirmado |
| `CreatedAt` | DateTime | Data de criação da conta |

## Refresh Tokens (Futuro)

Atualmente a implementação usa apenas Access Tokens. Para implementar Refresh Tokens:

1. Adicionar tabela de Refresh Tokens no banco
2. Criar endpoint `/api/auth/refresh`
3. Implementar lógica de renovação de tokens
4. Configurar expiração mais curta para Access Tokens (15 min)
5. Refresh Tokens com validade maior (7 dias)

## Troubleshooting

### Token inválido ou expirado
- Verifique se o token está sendo enviado corretamente no header `Authorization: Bearer {token}`
- Verifique se o token não expirou (campo `expiresAt`)
- Faça login novamente para obter um novo token

### Erro 401 Unauthorized
- Certifique-se de que o endpoint está protegido com `[Authorize]`
- Verifique se o token está presente no header
- Verifique se o usuário tem as permissões necessárias

### Erro ao criar migration
- Certifique-se de que o `AppDbContext` herda de `IdentityDbContext<ApplicationUser>`
- Verifique a connection string no `appsettings.json`

## Recursos Adicionais

- [Documentação ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity)
- [Documentação JWT Bearer](https://learn.microsoft.com/aspnet/core/security/authentication/jwt-authn)
- [Claims e Authorization](https://learn.microsoft.com/aspnet/core/security/authorization/claims)
