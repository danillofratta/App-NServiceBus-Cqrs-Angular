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

## TODO 18/02
- Review para verificar implmentações e refatorar caso necessário
- Implementar SignalIR para melhoria
- Implementar SerialLogg para melhoria
- Implementar possível log na base de dados registrando os status, o tabela especifica ou na tabela especifica

- Implementar Docker

