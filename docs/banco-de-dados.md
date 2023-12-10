# Banco de dados

Esse é um guia geral para criar uma conexão com banco de dados, independente 
do serviço.

Termos como `<nome-do-container-do-banco>`, `<nome-do-database>` e `<porta>`
devem ser substituídos pelos valores corretos que podem ser obtidos no
`docker-compose.yml` de cada serviço.

`<nome-do-container-do-banco>` pode ser `dnit-usuario-db`, `dnit-escola-db`
ou `dnit-ups-db`.

`<nome-do-database>` pode ser `usuarioservice`, `escolaservice` ou `upservice`.

Portas:

- 5444 -> dnit-escola-db
- 5433 -> dnit-ups-db
- 5432 -> dnit-usuario-db


## Acessando pelo PgAdmin em container Docker

Não é obrigatório executar UsuarioService pelo docker. Ainda assim, é 
necessário que o docker crie a rede usada pelos bancos de dados dos containeres
dos outros serviços. Para criar a rede, você pode iniciar apenas os containeres
do pgadmin e do banco de dados:

```sh
docker compose up -d <nome-do-container-do-banco> pgadmin 
```

Então você vai poder acessar um cliente PostgreSQL em http://localhost:5555.
Use o **email** `dnit@fga.com` e a **senha** `fga1234` para fazer login.

Para criar uma conexão com o banco de dados use as seguintes informações:

- Host: `<nome-do-container-do-banco>`
- Porta: 5432
- Database: `<nome-do-database>`
- Senha: 1234

## Acessando por um cliente PostgreSQL nativo

Instale o [Postbird](https://github.com/Paxa/postbird#download)
ou o [DBeaver](https://dbeaver.io/download/).

Para criar uma conexão com o banco de dados use as seguintes informações:

- Host: localhost
- Porta: `<porta>`
- Database: `<nome-do-database>`
- Senha: 1234

## Dicas gerais

Pode ser que você queira criar uma conexão com o banco de dados em produção.
Se esse for o caso, deixe explícito:

![banco prod](./banco-prod.png)

Assim você reduz as chances de executar queries no banco errado.