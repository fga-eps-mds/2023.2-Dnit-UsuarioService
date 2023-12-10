# Ambiente

## Windows e MacOs

### Modo 1

- Abra um navegador da web e acesse o site oficial da Microsoft .NET: 
https://dotnet.microsoft.com/download/dotnet/6.0
- Role a página até a seção ".NET 6 SDK" e clique no botão de download adequado 
para seu sistema operacional (por exemplo, "macOS x64 Installer" para macOS 64 bits
 ou  "Windows x64 Installer" para Windows 64 bits).
- O arquivo de instalação será baixado. Depois que o download for concluído, 
clique duas vezes no arquivo para iniciá-lo.
- O instalador será aberto. Leia e aceite os termos de licença.
- Selecione as opções de instalação que você deseja.
- Clique no botão "Install" (Instalar) para iniciar a instalação do .NET 6.
- Após a conclusão da instalação, você verá uma tela informando que o .NET 6 SDK
 foi instalado com sucesso.
- Para verificar se a instalação foi bem-sucedida, abra o Prompt de Comando ou o
 PowerShell e execute o seguinte comando:

```bash
dotnet --version
```

- Isso exibirá a versão do .NET instalada, confirmando se o .NET 6 está configurado corretamente.

### Modo 2

Basta instalar a IDE 
[Visual Studio](https://visualstudio.microsoft.com/pt-br/free-developer-offers/)
escolhendo a versão gratuita (Versão Community). Após instalar o Visual Studio, 
ele automaticamente irá instalar o .NET com a versão mais estável.

## Linux

Instale o SDK do .NET .

```bash
sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0
```

Instale o runtime ASP.NET Core.

```bash
sudo apt-get update && \
  sudo apt-get install -y aspnetcore-runtime-6.0
```

Entre na pasta do serviço. Dentro da pasta "app" rode o comando:

```bash
dotnet run
```

## Como Rodar

### Utilizando docker-compose

#### Usando Visual Studio

Para rodar uma aplicação usando Visual Studio, basta clicar no arquivo com 
extenção 'sln' e em seguida clicar no ícone para rodar aplicação conforme mostra
 abaixo:

![rodar](https://github.com/fga-eps-mds/2023.1-Dnit-EscolaService/assets/54676096/c7f08d0f-e1e7-45ab-b5a4-bbf1089ce1d8)

#### Usando Visual Studio Code

Para rodar utilizando o VS Code, basta seguir a seguinte instrução:

Entre na pasta do serviço. Dentro da pasta "app" rode o comando:

```bash
dotnet run
```

### Encerrando a aplicação

- No terminal em que a aplicação esta rodando, digite simultaneamente as teclas Ctrl + C. 
- Caso esteja utilizando o Visual Studio, clique no ícone quadrado vermelho.

![parar](https://github.com/fga-eps-mds/2023.1-Dnit-EscolaService/assets/54676096/45aedf91-bfb3-4475-afeb-6111a6feabe8)

## Documentação endpoints

Para documentar os endpoints estamos utilizando o Swagger. Caso queira visualizar, basta abrir a rota: 

```bash
http://localhost:7083/swagger/index.html
```

<img width="200" src="https://github.com/fga-eps-mds/2023.1-Dnit-UsuarioService/assets/54676096/2b2b5fef-7b52-4f40-ab91-c391aaae5d76" alt="swagger-usuarioservice" style="width:800px;">

