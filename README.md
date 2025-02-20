# Objetivo

Este projeto tem o objetivo de demonstrar a implementação das seguintes tecnologias e padrões: 
- API (.net) 
- Patterns: DDD, EDD, CQRS
- NServiceBus usando Saga (rabbitmq)
- Redis
- Fluent
- Mediatr
- Entity
- Angular
- Postgres
- Docker
- ...

## Projeto

São 4 serviços:
- Product, cadastro simples usando redis para cache
- Stock, cadastro simples entrada de produtos no estoque
- Sale, cadastro simples de uma venda. 
    - Processo (NServiceBus Saga):
        - Envia mensagem para serviço Stock
        - Serviço Stock verifica se tem os itens no Stock
        - Se sim, envia mensagem para Sale dizendo stock ok e envia mensagem para Payment
        - Se não, envia mensagem para Sale e encerra o processo
        - Payment simula o pagamento ok ou fail, nos dois casos retorna comunicação para Sale e encerra o processo       
- Payment, simula pagamento ok ou fail e envia mensagem para Sale
- Contrução de um mini framework para implementação

## Rodar Aplicação no Docker

- build do backend, acessar a pasta src\backend\src e no terminal rodar
    - docker-compose up --build
- build do fronteend, acessar a pasta src\frontend\appclientangular e no terminal rodar
    - docker build -t app-client -f Dockerfile .
- rodar migration para gerar o banco de dados. Acessar src\backend\src\Shared\Shared.Infrasctructure e no terminal rodar
    - dotnet ef migrations add InitialCreate
    - dotnet ef database update
- rodar os containers

## TODO
- criar diagramas arquitetura do projeto
- criar diagramas fluxo do processo
- criar IoC
- revisar projeto
- add mais consultas com signalr

