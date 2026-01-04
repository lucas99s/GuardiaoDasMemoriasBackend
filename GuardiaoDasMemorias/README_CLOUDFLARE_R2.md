# Configuração do Cloudflare R2 para Upload de Músicas

## ?? Pré-requisitos

1. Conta no Cloudflare
2. Cloudflare R2 habilitado

## ?? Configuração

### 1. Criar um Bucket no Cloudflare R2

1. Acesse o [Cloudflare Dashboard](https://dash.cloudflare.com/)
2. Vá em **R2** no menu lateral
3. Clique em **Create bucket**
4. Nomeie o bucket (ex: `guardiaodasmemorias`)
5. Clique em **Create bucket**

### 2. Obter as Credenciais de Acesso

1. No painel R2, vá em **Manage R2 API Tokens**
2. Clique em **Create API token**
3. Configure as permissões:
   - **Permissions**: Admin Read & Write
   - **Apply to**: All buckets
4. Clique em **Create API Token**
5. **IMPORTANTE**: Copie e salve:
   - `Access Key ID`
   - `Secret Access Key`
   - `Account ID` (visível no dashboard)

### 3. Configurar Acesso Público

1. No bucket criado, vá em **Settings**
2. Em **Public access**, clique em **Allow access**
3. (Opcional) Configure um domínio personalizado em **Custom Domains**

### 4. Configurar o appsettings.json

```json
{
  "CloudflareR2": {
    "AccountId": "seu-account-id",
    "AccessKeyId": "sua-access-key-id",
    "SecretAccessKey": "sua-secret-access-key",
    "BucketName": "guardiaodasmemorias",
    "PublicUrl": "https://seu-dominio.com"
  }
}
```

### 5. Variáveis de Ambiente (Produção)

Para produção, configure as variáveis de ambiente:

```bash
CLOUDFLARER2__ACCOUNTID=seu-account-id
CLOUDFLARER2__ACCESSKEYID=sua-access-key-id
CLOUDFLARER2__SECRETACCESSKEY=sua-secret-access-key
CLOUDFLARER2__BUCKETNAME=guardiaodasmemorias
CLOUDFLARER2__PUBLICURL=https://seu-dominio.com
```

## ?? Uso da API

### Upload de Música

**Endpoint**: `POST /api/musica/upload`

**Content-Type**: `multipart/form-data`

**Parâmetros:**
- `arquivo` (file): Arquivo de áudio (MP3, WAV, OGG, WEBM)
- `nome` (string): Nome da música
- `memoriaId` (int): ID da memória associada

**Exemplo com cURL:**

```bash
curl -X POST "https://sua-api.com/api/musica/upload" \
  -F "arquivo=@caminho/para/musica.mp3" \
  -F "nome=Minha Música Favorita" \
  -F "memoriaId=1"
```

**Exemplo com JavaScript:**

```javascript
const formData = new FormData();
formData.append('arquivo', arquivoInput.files[0]);
formData.append('nome', 'Minha Música Favorita');
formData.append('memoriaId', 1);

const response = await fetch('https://sua-api.com/api/musica/upload', {
  method: 'POST',
  body: formData
});

const result = await response.json();
```

**Resposta de Sucesso (201 Created):**

```json
{
  "id": 1,
  "nome": "Minha Música Favorita",
  "caminho": "https://seu-dominio.com/550e8400-e29b-41d4-a716-446655440000_musica.mp3",
  "memoriaId": 1
}
```

### Deletar Música

**Endpoint**: `DELETE /api/musica/{id}`

Ao deletar uma música, o arquivo será automaticamente removido do R2.

## ?? Configurações

- **Tamanho máximo do arquivo**: 50MB
- **Formatos aceitos**: MP3, WAV, OGG, WEBM
- **Armazenamento**: Cloudflare R2 (sem taxas de egress)

## ?? Segurança

?? **IMPORTANTE**:
- Nunca commite suas credenciais no repositório
- Use variáveis de ambiente em produção
- Adicione `appsettings.Development.json` ao `.gitignore`

## ?? Custos

Cloudflare R2 oferece:
- **10 GB/mês de armazenamento gratuito**
- **Sem taxas de egress** (transferência de dados)
- Operações de Classe A: 1 milhão/mês grátis
- Operações de Classe B: 10 milhões/mês grátis

Para mais informações: [Cloudflare R2 Pricing](https://developers.cloudflare.com/r2/pricing/)
