# Vroom API
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) 

Esse é o projeto Vroom para o Challenge FIAP 2025, onde os usuários podem registrar motos, tags e manipular as tags comunicando com o IOT

## Funcionalidades

- Registro de eventos IoT
- Gerenciamento de motocicletas
- Controle de tags de localização
- Sistema de monitoramento e diagnóstico

# Documentação da API

## IoT

### /historico

```http
POST /historico
```

Recebe e processa um evento IoT

| Parâmetro   | Tipo      | Descrição                                    |
| :---------- | :-------- | :------------------------------------------- |
| `idTag`     | `integer` | **Obrigatório**. ID da tag associada        |
| `timestamp` | `string`  | **Obrigatório**. Timestamp do evento        |
| `ledOn`     | `boolean` | Indica se o LED está ligado                  |
| `problema`  | `string`  | Descrição do problema detectado              |
| `cor`       | `integer` | Código da cor do LED (0-255)                |

**Exemplo de Request Body (JSON)**

```json
{
  "idTag": 1,
  "timestamp": "2024-01-15T10:30:00",
  "ledOn": true,
  "problema": "Temperatura alta detectada",
  "cor": 1
}
```

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `200`  | OK        | Evento IoT criado |
| `400`  | Dados inválidos | Mensagem de erro |
| `404`  | Tag não encontrada | Mensagem de erro |

### /Iot

```http
GET /Iot
```

Retorna todos os eventos IoT com paginação

| Parâmetro  | Tipo      | Descrição                          |
| :--------- | :-------- | :--------------------------------- |
| `page`     | `integer` | Número da página (padrão: 1)       |
| `pageSize` | `integer` | Itens por página (padrão: 10)      |

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `200`  | OK        | Lista de eventos IoT |

## Motocicletas

### /Moto

```http
POST /Moto
```

Cria uma nova moto no sistema

| Parâmetro           | Tipo       | Descrição                               |
| :------------------ | :--------- | :-------------------------------------- |
| `placa`             | `string`   | **Obrigatório**. Placa da moto          |
| `chassi`            | `string`   | **Obrigatório**. Número do chassi       |
| `descricaoProblema` | `string`   | **Obrigatório**. Descrição do problema  |
| `modeloMoto`        | `integer`  | **Obrigatório**. Modelo da moto (0-2)   |
| `categoriaProblema` | `integer`  | **Obrigatório**. Categoria do problema (0-6) |
| `tagId`             | `integer`  | **Obrigatório**. ID da tag associada    |

**Modelos de Moto:**
- `0` - MOTTUPOP (Modelo básico)
- `1` - MOTTUSPORT (Modelo esportivo)
- `2` - MOTTUE (Modelo elétrico)

**Categorias de Problema:**
- `0` - MECANICO
- `1` - ELETRICO
- `2` - DOCUMENTACAO
- `3` - ESTETICO
- `4` - SEGURANCA
- `5` - MULTIPLO
- `6` - CONFORME

**Exemplo de Request Body (JSON)**

```json
{
  "placa": "ABC-1234",
  "chassi": "9BWZZZ377VT004251",
  "descricaoProblema": "Motor fazendo ruído estranho",
  "modeloMoto": 0,
  "categoriaProblema": 0,
  "tagId": 1
}
```

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `201`  | Criada    | Moto criada com sucesso |
| `400`  | Dados inválidos | Mensagem de erro |

### /Moto/{id}

```http
GET /Moto/{id}
```

Busca uma moto específica pelo ID

| Parâmetro | Tipo      | Descrição                              |
| :-------- | :-------- | :------------------------------------- |
| `id`      | `integer` | **Obrigatório**. ID da moto desejada   |

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `200`  | OK        | Dados da moto |
| `404`  | Não encontrada | Mensagem de erro |

### /Moto

```http
GET /Moto
```

Retorna todas as motos cadastradas no sistema

| Parâmetro  | Tipo      | Descrição                          |
| :--------- | :-------- | :--------------------------------- |
| `page`     | `integer` | Número da página (padrão: 1)       |
| `pageSize` | `integer` | Itens por página (padrão: 10)      |

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `200`  | OK        | Lista de motos |

### /Moto/{id}

```http
PUT /Moto/{id}
```

Atualiza os dados de uma moto existente

| Parâmetro           | Tipo       | Descrição                               |
| :------------------ | :--------- | :-------------------------------------- |
| `id`                | `integer`  | **Obrigatório**. ID da moto             |
| `placa`             | `string`   | **Obrigatório**. Placa da moto          |
| `chassi`            | `string`   | **Obrigatório**. Número do chassi       |
| `descricaoProblema` | `string`  | **Obrigatório**. Descrição do problema  |
| `modeloMoto`        | `integer`  | **Obrigatório**. Modelo da moto         |
| `categoriaProblema` | `integer`  | **Obrigatório**. Categoria do problema  |
| `tagId`             | `integer`  | **Obrigatório**. ID da tag associada    |

**Exemplo de Request Body (JSON)**

```json
{
  "placa": "XYZ-5678",
  "chassi": "9BWZZZ377VT004251",
  "descricaoProblema": "Problema no freio traseiro",
  "modeloMoto": 1,
  "categoriaProblema": 4,
  "tagId": 2
}
```

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `200`  | OK        | Moto atualizada |
| `404`  | Não encontrada | Mensagem de erro |

### /Moto/{id}

```http
DELETE /Moto/{id}
```

Remove uma moto do sistema

| Parâmetro | Tipo      | Descrição                                |
| :-------- | :-------- | :--------------------------------------- |
| `id`      | `integer` | **Obrigatório**. ID da moto a ser removida |

**Respostas**

| Código | Descrição |
| :----- | :-------- |
| `204`  | Removida com sucesso |
| `404`  | Não encontrada |

## Tags de Localização

### /Tag

```http
POST /Tag
```

Cria uma nova tag no sistema

| Parâmetro    | Tipo     | Descrição                                    |
| :----------- | :------- | :------------------------------------------- |
| `coordenada` | `string` | **Obrigatório**. Coordenadas (lat,long)     |
| `disponivel` | `byte`   | **Obrigatório**. Status (0=indisponível, 1=disponível) |

**Exemplo de Request Body (JSON)**

```json
{
  "coordenada": "-23.5505,-46.6333",
  "disponivel": 1
}
```

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `201`  | Criada    | Tag criada com sucesso |
| `400`  | Dados inválidos | Mensagem de erro |

### /Tag/{id}

```http
GET /Tag/{id}
```

Busca uma tag específica pelo ID

| Parâmetro | Tipo      | Descrição                              |
| :-------- | :-------- | :------------------------------------- |
| `id`      | `integer` | **Obrigatório**. ID da tag desejada    |

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `200`  | OK        | Dados da tag |
| `404`  | Não encontrada | Mensagem de erro |

### /Tag

```http
GET /Tag
```

Retorna todas as tags cadastradas no sistema

| Parâmetro  | Tipo      | Descrição                          |
| :--------- | :-------- | :--------------------------------- |
| `page`     | `integer` | Número da página (padrão: 1)       |
| `pageSize` | `integer` | Itens por página (padrão: 10)      |

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `200`  | OK        | Lista de tags |

### /Tag/{id}

```http
PUT /Tag/{id}
```

Atualiza os dados de uma tag existente

| Parâmetro    | Tipo      | Descrição                                    |
| :----------- | :-------- | :------------------------------------------- |
| `id`         | `integer` | **Obrigatório**. ID da tag                   |
| `coordenada` | `string`  | **Obrigatório**. Coordenadas (lat,long)     |
| `disponivel` | `byte`    | **Obrigatório**. Status (0=indisponível, 1=disponível) |

**Exemplo de Request Body (JSON)**

```json
{
  "coordenada": "-23.5505,-46.6333",
  "disponivel": 0
}
```

**Respostas**

| Código | Descrição | Conteúdo |
| :----- | :-------- | :---- |
| `200`  | OK        | Tag atualizada |
| `404`  | Não encontrada | Mensagem de erro |

### /Tag/{id}

```http
DELETE /Tag/{id}
```

Remove uma tag do sistema

| Parâmetro | Tipo      | Descrição                                |
| :-------- | :-------- | :--------------------------------------- |
| `id`      | `integer` | **Obrigatório**. ID da tag a ser removida |

**Respostas**

| Código | Descrição |
| :----- | :-------- |
| `204`  | Removida com sucesso |
| `404`  | Não encontrada |

## Variáveis de Ambiente

Para rodar esse projeto, você vai precisar adicionar as seguintes variáveis de ambiente no appsettings.json

```json
{
  "ConnectionStrings": {
    "OracleConnection": "Data Source=ConexaoBanco;User Id=seuRM;Password=suaSenha"
  }
}
```

## Como Executar

1. Clone o repositório
2. Configure as variáveis de ambiente no `appsettings.json`
3. Execute o comando: `dotnet restore`
4. **Configure o banco de dados:**
   - Certifique-se de que sua string de conexão Oracle está correta no `appsettings.json`
   - Execute as migrations para criar/atualizar o banco de dados:
     ```bash
     dotnet ef database update
     ```
   - Caso não tenha o EF Tools instalado, instale com:
     ```bash
     dotnet tool install --global dotnet-ef
     ```
5. Execute o comando: `dotnet run`
6. Acesse a documentação da API em: `https://localhost:5001/swagger`

## Arquitetura

O projeto utiliza:
- **ASP.NET Core** - Framework principal
- **Oracle Database** - Banco de dados
- **Entity Framework Core** - ORM
- **AutoMapper** - Mapeamento de objetos
- **Swagger/OpenAPI** - Documentação da API

### Justificativa da Arquitetura

#### **Padrão de Arquitetura Escolhido: Clean Architecture**

O projeto adota a **Clean Architecture**, garantindo separação de responsabilidades, baixo acoplamento e alta testabilidade.
### Camadas principais

Controllers: Recebem requisições HTTP

- Services: Lógica de negócio
- DTOs: Transferência de dados
- Models: Entidades do domínio
- Interfaces: Contratos para inversão de dependência

#### **Estrutura das Camadas**

1. **Controllers**: Responsáveis por receber as requisições HTTP e orquestrar as operações
2. **Services**: Contêm a lógica de negócio e regras específicas do domínio
3. **DTOs (Data Transfer Objects)**: Objetos para transferência de dados entre camadas
4. **Models**: Representam as entidades do domínio
5. **Interfaces**: Definem contratos para inversão de dependência

#### **Tecnologias Utilizadas**

- **ASP.NET Core**: Framework robusto para APIs REST
- **Oracle + EF Core**: Persistência de dados com ORM e migrations
- **AutoMapper**: Conversão automática entre objetos
- **Swagger/OpenAPI**: Documentação interativa da API

#### **Benefícios da Arquitetura Implementada**

- **Escalabilidade**: Estrutura modular e assíncrona
- **Manutenibilidade**: Código organizado e fácil de evoluir
- **Testabilidade**: Camadas desacopladas com suporte a mocks
- **Flexibilidade**: Fácil troca de implementações e integração de novas features

#### **Padrões de Design Aplicados**

- **Repository Pattern**: Abstração da camada de dados
- **Service Pattern**: Encapsulamento da lógica de negócio
- **Dependency Injection**: Inversão de controle para baixo acoplamento
- **DTO Pattern**: Controle sobre dados transferidos entre camadas
- **Result Pattern**: Tratamento consistente de erros e sucessos

## Autores

- [@Gabriel Marcello](https://github.com/gabrielmarcello) RM556783
- [@Guilherme Guimarães](https://github.com/Guimaraes131) RM557074
- [@Matheus Luna](https://github.com/mlunahodov) RM555547

