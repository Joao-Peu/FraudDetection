# FraudDetection

Este projeto tem como objetivo a detecção de fraudes em transações financeiras utilizando RabbitMQ para o gerenciamento de mensagens e comunicação entre serviços. Ele permite a leitura e o processamento de dados em tempo real, usando filas de mensagens para garantir a escalabilidade e eficiência no processamento de transações.

## Funcionalidades

- **Conexão com RabbitMQ**: Estabelece uma conexão com RabbitMQ para gerenciar filas e trocas de mensagens.
- **Processamento de Transações**: Recebe e processa transações financeiras, realizando a detecção de fraudes.
- **Desempenho**: A arquitetura baseada em mensagens garante alta performance e baixa latência no processamento de dados.

## Tecnologias

- **RabbitMQ**: Sistema de mensageria para comunicação entre sistemas.
- **.NET Core**: Framework utilizado para o desenvolvimento da aplicação.
- **Swagger**: Para documentação e testes da API.
- **Entity Framework Core**: Para o gerenciamento de dados no banco de dados.

## Como Executar

1. Clone o repositório.
   ```bash
   git clone https://github.com/Joao-Peu/FraudDetection.git
2. Abra o projeto no Visual Studio.
3. Execute o projeto para iniciar o serviço de detecção de fraudes.
