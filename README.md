# GestaoClientes.Legacy — Laboratório SDD e Codex

Projeto **ASP.NET Core MVC (.NET 10)** monolítico, com Entity Framework Core 10 e SQL Server. Preparado como ponto de partida para uma aula de modernização arquitetural com Codex: extrair Clientes e Planos em microsserviços e manter Contratos temporariamente no legado.

## Pré-requisitos

- Docker Desktop com containers Linux, preferencialmente 4 GB+ de RAM disponíveis para o SQL Server.
- Para executar fora de Docker: .NET SDK 10.

## Caminho mais simples: subir a aplicação inteira no Docker

A partir da raiz do repositório:

```bash
docker compose up -d --build
docker compose ps
```

Abra **http://localhost:8080**. Na primeira inicialização, o SQL Server pode levar alguns instantes para estar pronto. O projeto executa `EnsureCreated()` e gera planos, clientes e um contrato de exemplo automaticamente.

## Executar MVC pelo Visual Studio ou terminal

1. Inicie só o banco: `docker compose up -d sqlserver`.
2. Abra `src/GestaoClientes.Mvc/GestaoClientes.Mvc.csproj` no Visual Studio 2022 atualizado, com suporte ao .NET 10.
3. Ou execute `dotnet run --project src/GestaoClientes.Mvc --launch-profile http`.
4. Acesse **http://localhost:5080**.

String de conexão local: SQL Server `localhost,1434`, banco `DbGestaoClientes`, login `sa`; senha de laboratório indicada no `appsettings.json`. Utilize credenciais diferentes fora deste exercício.

## Funcionalidades

- Dashboard com quantidade de clientes, planos e contratos.
- Clientes: listar, buscar, cadastrar, editar e desativar. CPF único, 11 dígitos, e plano ativo obrigatório.
- Planos: listar, cadastrar, editar; nome único e impedimento de desativar plano com cliente ativo.
- Contratos: listar, cadastrar e editar; relacionamento com cliente ativo.
- Views Razor responsivas com Bootstrap 5.3 (CDN).
- Banco SQL Server único, com três tabelas relacionadas e dados iniciais.

## Acoplamentos intencionais para a aula

1. Controllers acessam `GestaoDbContext` diretamente.
2. As entidades possuem navegações entre módulos.
3. Relacionamentos usam chaves estrangeiras no mesmo banco.
4. Regras de validação de negócio estão dentro dos controllers.
5. Contratos realiza consulta encadeada Cliente -> Plano.
6. Inicialização e seed do banco estão ligados ao `Program.cs`.

Esses pontos são **intencionais** e não devem ser corrigidos antes do exercício: o Codex deve identificar e planejar a mudança orientada pelas especificações.

## Roteiro sugerido

1. Versione o monólito com Git, e valide as funcionalidades.
2. Peça ao Codex análise arquitetural **sem mudar código**.
3. Crie `constitution.md`, `requirements.md`, `specification.md`, `architecture.md`, `api-contracts.md` e `tasks.md`.
4. Extraia `ApiClientes` e seu banco, mantendo a interface MVC.
5. Extraia `ApiPlanos`; substitua a FK entre bancos por referência lógica e contrato HTTP.
6. Substitua acesso direto do MVC ao banco por `HttpClientFactory`.
7. Adicione Gateway com YARP e serviços Docker.
8. Preserve Contratos no monólito para demonstrar a migração incremental.

Consulte `docs/DESAFIO-SDD.md` para critérios de aceitação.

## Atenção

- Este é um projeto **didático**, não pronto para produção. Não inclui autenticação, autorização, testes automatizados ou migrations EF, justamente para criar espaço para as atividades de arquitetura e qualidade.
- A estratégia `EnsureCreated()` é apenas para o banco inicial descartável. Não oferece migrações de esquema; nunca use para modificar bases existentes de produção.
- O SQL Server do Compose é para desenvolvimento e tem porta publicada para facilitar a aula.
- Para limpar todos os dados do laboratório: `docker compose down -v` (destrutivo).
- As dependências NuGet serão restauradas no primeiro build e o Bootstrap usa CDN, exigindo conexão de rede.
