Movies API - Módulo Admin (MoviesAPIAdminModule)
Este repositório contém o Módulo de Administração (Write Database) de uma robusta plataforma de gerenciamento de filmes. O projeto foi construído utilizando .NET 10 (migrado em 12/11/2025) e segue uma arquitetura de microsserviços, sendo este o serviço principal de "escrita" e gerenciamento de dados.
O objetivo deste módulo é ser o "source of truth" (fonte da verdade) de todo o sistema. Ele se comunica com um banco de dados SQL Server e é responsável por todas as operações de criação, atualização, exclusão e gerenciamento de permissões.

🚀 Funcionalidades Atuais (Admin)
*Como módulo de administração, esta API foca em operações seguras e validadas:
*Gerenciamento de Filmes: CRUD completo para filmes, incluindo seus relacionamentos.
*Gerenciamento de Metadados: CRUD para Gêneros, Diretores, Estúdios e Prêmios.
*Gerenciamento de Acesso: Permite a administradores gerenciarem roles de usuários e cadastrarem novos administradores na plataforma.
*Busca Otimizada: Listagem e paginação eficientes de todos os itens, utilizando IQueryable e Entity Framework.

🏛️ Arquitetura e Design (DDD + Clean Architecture)
*Este módulo foi projetado com uma arquitetura Clean Architecture adaptada para os conceitos de Domain-Driven Design (DDD), focando em robustez, manutenibilidade e regras de negócio claras.

Conceitos de DDD Aplicados:
*Entidades e Agregados: As entidades (como Movie) contêm suas próprias regras de negócio e validam seu estado, protegendo a consistência dos dados.
*Value Objects (VOs): Objetos imutáveis (como MovieImage e Award) são usados para atributos complexos e implementam lógica de comparação.
*Validação em Camadas:
*Validação Interna: As próprias Entidades e VOs se protegem contra estados inválidos.
*Validação Externa: O Fluent Validations é usado na camada de Aplicação para validar DTOs de entrada.

Stack Técnica e Padrões:
*Result Pattern: Todo o fluxo da aplicação, dos Handlers aos Controllers, utiliza o Padrão Result para um tratamento de erros explícito e robusto, eliminando a necessidade de exceções para controle de fluxo.
*Mediator (Manual): O padrão Mediator foi implementado manualmente (sem bibliotecas externas) para orquestrar os use cases (Commands e Queries), garantindo baixo acoplamento.
*DTOs Imutáveis: Todos os DTOs de entrada e saída são implementados como record types do C#.
*Testes: Cobertura de testes unitários para todas as camadas (Domínio, Aplicação, Infraestrutura).
*Documentação: A API utiliza NSwag (substituindo o Swashbuckle) para uma documentação moderna e totalmente configurada para suportar Versionamento e autenticação JWT.
*Armazenamento de Mídia: O sistema está configurado para salvar imagens localmente (ativo) e possui a abstração para salvar em um bucket Amazon S3.
*Containerização: O projeto inclui Dockerfile e docker-compose.yml prontos para ambientes de desenvolvimento e produção.

🗺️ Visão Futura do Sistema
*Este Módulo Admin é o primeiro de dois microsserviços. O fluxo de dados completo será:
*Módulo Admin (Este projeto): Salva dados no SQL Server (Banco de Escrita).
*Mensageria: O Admin publicará eventos (ex: "Filme Criado") em um message broker.
*Módulo Catalog (Futuro): Um segundo serviço (lendo do MongoDB para alta performance de leitura) irá consumir essas mensagens para sincronizar os dados e servir o catálogo público.
