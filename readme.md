Desafio Luiza Labs!

##Pre requisitos
> Sdk=Microsoft.NET.Sdk.Web
> Sdk=Microsoft.NET.Sdk
> TargetFramework netcoreapp3.1

##Passos

1. - Fazer o git clone de https://github.com/rodboza/TesteLuizaLabs.git
2. - Na raiz do repositório entrar com os comandos abaixo
	2.1. - .\TesteLuizaLabs> dotnet restore
	2.2. - .\TesteLuizaLabs> dotnet build
	2.3. - .\TesteLuizaLabs> dotnet run --project .\src\Favoritos.API\Favoritos.API.csproj --configuration Release
3 - Será exibido na tela 
	info: Microsoft.Hosting.Lifetime[0]
		  Now listening on: https://localhost:5001
	info: Microsoft.Hosting.Lifetime[0]
		  Now listening on: http://localhost:5000
	info: Microsoft.Hosting.Lifetime[0]
		  Application started. Press Ctrl+C to shut down.
4 - Caso queira fazer uma carga inicial de dadaos, usar https://localhost:5001/api/v1/cargainicial
	4.1 - Nessa carga será criado um usuário de login teste com a senha 123
5 - Para criar um usuário de login use
	5.1 - https://localhost:5001/api/v1/loging/registrar
	5.2 - Verbo POST
	5.3 - Body Json {"UserName":"teste","Password":"123"} 
6 - Para todas as chamadas você precisa passar um token do tipo BearerToke no header da chamada 
	6.1 - Para gerar um token use 
		6.1.1 - Caminho  https://localhost:5001/api/v1/loging 
		6.1.2 - Verbo POST
		6.1.3 - Body Json {"UserName":"teste","Password":"123"} (caso tenha feito a carga inicial)
	6.2 - Para testar o token use
		6.2.1 - Caminho https://localhost:5001/api/v1/login/teste
		6.2.2 - Verbo Get
7 - CRUD do Cliente https://localhost:5001/api/v1/cliente
	7.1 - Criar - POST api/v1/cliente/ com body {"Nome":"teste","Email":"test3e@teste.com"}
	7.2 - Ler
		7.2.1 Todos - GET api/v1/cliente/
		7.2.2 Por ID - GET api/v1/cliente/7735578f-3da2-4d4a-be77-b17ead76f5d ( funciona com a cargainicial)
	7.3 - Atualizar - PUT  api/v1/cliente/7735578f-3da2-4d4a-be77-b17ead76f5d com body {"Nome":"teste","Email":"test3e@teste.com"}
	7.4 - Remover - DELETE api/v1/cliente/7735578f-3da2-4d4a-be77-b17ead76f5d
8 - Lista de Favoritos https://localhost:5001/api/v1/cliente/{ID_CLIENTE}/Favorito
	8.1 - ATENÇÃO - o ID_PRODUTO é o Id fornecido na API http://challenge-api.luizalabs.com/api/product 
	8.2 - Inclusão - POST api/v1/cliente/{ID_CLIENTE}/Favorito com body {"idProduto":"571fa8cc-2ee7-5ab4-b388-06d55fd8ab2f"}
	8.3 - Remover - DELETE api/v1/cliente/{ID_CLIENTE}/Favorito/{ID_PRODUTO}
	8.4 - Ler - GET api/v1/cliente/{ID_CLIENTE}/Favorito
