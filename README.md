# MotoTrack 🏍️📡

**MotoTrack** é uma API RESTful desenvolvida com **ASP.NET Core 8.0** que permite o rastreamento de motocicletas por meio de leitores **RFID**.  
O sistema gerencia motos, leitores e registros de passagem em pontos monitorados, utilizando **Entity Framework Core** com **banco de dados Oracle**.

---

## Equipe do Projeto

| **Angello Turano** **RM: 556511** | **Cauã Sanches** **RM:558317** | **Leonardo Bianchi** **RM:558576** |

---

## 📦 Tecnologias Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core
- Oracle Database
- Swagger/OpenAPI
- C#

---

## 📐 Estrutura do Projeto

- `Moto`: representa uma motocicleta (placa, modelo, status, etc.)
- `LeitorRFID`: representa um leitor fixado em um ponto de controle
- `RegistroLeituraRFID`: registro de passagem de uma moto por um leitor

---

## 📁 Endpoints Disponíveis

### 🔧 Motos

| Método | Rota              | Descrição                   |
| ------ | ----------------- | --------------------------- |
| GET    | `/api/motos`      | Lista todas as motos        |
| GET    | `/api/motos/{id}` | Retorna uma moto específica |
| POST   | `/api/motos`      | Cadastra uma nova moto      |
| PUT    | `/api/motos/{id}` | Atualiza uma moto existente |
| DELETE | `/api/motos/{id}` | Remove uma moto do sistema  |

### 📍 Leitores RFID

| Método | Rota                 | Descrição                    |
| ------ | -------------------- | ---------------------------- |
| GET    | `/api/leitores`      | Lista todos os leitores      |
| GET    | `/api/leitores/{id}` | Retorna um leitor específico |
| POST   | `/api/leitores`      | Cadastra um novo leitor      |
| PUT    | `/api/leitores/{id}` | Atualiza um leitor existente |
| DELETE | `/api/leitores/{id}` | Remove um leitor do sistema  |

### 📝 Registros de Leitura

| Método | Rota                  | Descrição                      |
| ------ | --------------------- | ------------------------------ |
| GET    | `/api/registros`      | Lista todos os registros       |
| GET    | `/api/registros/{id}` | Retorna um registro específico |
| POST   | `/api/registros`      | Registra passagem de uma moto  |
| DELETE | `/api/registros/{id}` | Remove um registro do sistema  |

---

## 🗃️ Modelos de Dados

### Moto

```json
{
  "id": 0,
  "placa": "ABC1234",
  "modelo": "Honda CG 160",
  "status": "Disponível",
  "leitorRFIDId": 1
}
```

### LeitorRFID

```json
{
  "id": 0,
  "nome": "Portão Principal",
  "localizacao": "Entrada da empresa"
}
```

### RegistroLeituraRFID

```json
{
  "id": 0,
  "motoId": 1,
  "leitorRFIDId": 1
}
```

---

## 🔄 Configuração do Banco Oracle

Adicione a string de conexão no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=seu_datasource"
  }
}
```

> 💡 Certifique-se de ter o **Oracle Data Provider** (`Oracle.EntityFrameworkCore`) instalado no projeto.

---

## ⚙️ Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
- Banco de dados Oracle acessível
- Oracle Data Provider (`Oracle.EntityFrameworkCore`) adicionado ao projeto
- Ferramenta de migrations do EF Core (opcional, para aplicar migrations)

---

## ▶️ Como Rodar o Projeto

1. Clone o repositório:

   ```bash
   git clone https://github.com/seu-usuario/mototrack.git
   cd mototrack
   ```

2. Configure a string de conexão no arquivo `appsettings.json` conforme as credenciais do seu banco Oracle.

3. (Opcional) Aplique as migrations ao banco de dados:

   ```bash
   dotnet ef database update
   ```

4. Restaure os pacotes:

   ```bash
   dotnet restore
   ```

5. Compile o projeto:

   ```bash
   dotnet build
   ```

6. Execute a aplicação:

   ```bash
   dotnet run
   ```

7. Acesse a documentação via Swagger:
   ```
   https://localhost:{porta}/swagger
   ```

---

## ✅ Funcionalidades Finais

- CRUD de motos, leitores e registros
- Associação entre motos e leitores
- Registro de movimentações com data e hora
- Interface de testes via Swagger

---

Desenvolvido para a disciplina **Advanced Business Development with .NET** 🧠

Link para o repositório: https://github.com/AngelloTDC/MotoTrack-.NET
