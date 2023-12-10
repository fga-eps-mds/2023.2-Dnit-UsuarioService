# UsuarioService

Serviço responsável pelas funcionalidades relacionadas à autenticação 
(cadastro de usuários, login, solicitação de recuperação de senha, redefinição de senha)
e autorização.

## Pré requisitos

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Docker e [Docker Compose](https://docs.docker.com/compose/install/)

Se você tem o Compose instalado como standalone, use:

```sh
docker-compose ...
```

Entretando, prefira instalar o Docker Compose como plugin (recomedação do 
Docker). Os comandos nesse formato são assim:

```sh
docker compose ...
```

Se você precisa de `sudo` para executar comandos do Docker, consulte o 
[guia](https://docs.docker.com/engine/install/linux-postinstall/) de pós-instação.

## Executar

```sh
git clone https://github.com/fga-eps-mds/2023.2-Dnit-UsuarioService.git
```

Execute com Docker:

```sh
docker compose up -d
```

Ou entre no diretório `app/` e inicie o servidor execute nativamente:

```sh
cd app
dotnet watch
```

Acesse a documentação pelo swagger em http://localhost:7083/swagger.

## Editor

Para mais informações sobre instalação e IDE, leia [ambiente.md](docs/ambiente.md).

### Licença

O projeto UsuarioService está sob as regras aplicadas na licença 
[AGPL-3.0](https://github.com/fga-eps-mds/2023.1-Dnit-UsuarioService/blob/main/LICENSE).

## Contribuidores

- [Daniel Porto](https://github.com/DanielPortods)
- [Rafael Berto](https://github.com/RafaelBP02)
- [Thiago Sampaio](https://github.com/thiagohdaqw)
- [Victor Hugo](https://github.com/victorhugo21)
- [Vitor Lamego](https://github.com/VitorLamego)
- [Wagner Martins](https://github.com/wagnermc506)
- [Yudi Yamane](https://github.com/yudi)
- [André Emanuel](https://github.com/Hunter104)
- [Artur Henrique](https://github.com/H0lzz)
- [Cássio Sousa](https://github.com/csreis72)
- [Eduardo Matheus](https://github.com/DiceRunner714)
- [João Antonio](https://github.com/joaoseisei)
- [Lucas Gama](https://github.com/bottinolucas)
- [Márcio Henrique](https://github.com/DeM4rcio)
