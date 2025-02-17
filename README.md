# Objetivo

Este projeto tem o objetivo de demonstrar a implementação das seguintes tecnologias e padrões: .net, webapi, DDD, EDD, CQRS, serviceBus (rabbitmq), Redis, Fluent, Mediatr, Docker, entity, Angular, postgres, ...

## Projeto

São 3 serviços:
- Product, cadastro simples usando redis para cache
- Stock, cadastro simples entrada de produtos no estoque
- Sale, cadastro simples de uma venda que faz a comunicação com Stock (NserviceBus). Serviço Stock faz o processo e retorna (NserviceBus) para serviço Sale
- Contrução de um mini framework para implementação

## TODO 17/02
- Refatorar
- Finalizar
- Implementar Docker

