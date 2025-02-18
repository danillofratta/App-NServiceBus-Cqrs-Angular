# Objetivo

Este projeto tem o objetivo de demonstrar a implementação das seguintes tecnologias e padrões: 
- .net, webapi, 
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
    - Processo:
        - Envia mensagem para serciço Stock
        - Serviço Stock verifica se tem os itens no Stock
        - Se sim envia mensagem para Sale dizendo ok e envia mensagem para Payment
        - Se não envia mensange para Sale e encerra o processo
        - Payment simula o pagamento ok ou fail, nos dois casos retorna comunicação para Sale e encerra o processo       
- Payment, simula pagamento ok ou fail e envia mensagem para Sale
- Contrução de um mini framework para implementação

## TODO 17/02
<<<<<<< HEAD
- Review para verificar implmentações e refatorar caso necessário
- Implementar SignalIR para melhoria
- Implementar SerialLogg para melhoria
- Implementar possível log na base de dados registrando os status, o tabela especifica ou na tabela especifica
=======
>>>>>>> aef7bc859e7cf8d4323852e10d0eec0d20f88316
- Implementar Docker

