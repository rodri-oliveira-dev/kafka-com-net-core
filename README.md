# Kafka com Net Core

Este projeto aborda o uso de Apache Kafka com NET Core.  

Projeto com um um producer e um consumer a nivel de aprendizado. Necessario pre-instalação do docker e docker compose.


## 1) Rodar o Docker Compose

No prompt de comando, no diretório raiz, use o commando para criar o ambiente:

```PowerShell
docker-compose up -d
```
Após a criação dos containers poe executar o comando abaixo para validar se os containers estão ok.

```PowerShell
docker-compose ps
```

## 2) Rodar o Docker Compose
Rodar uma instancia do producer e uma ou mais instancias do consumer.