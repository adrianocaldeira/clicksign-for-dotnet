## Clicksign for .Net

Biblioteca em .Net para consumo dos serviços da Clicksign.

A Clicksign é uma solução online para enviar, guardar e assinar documentos, com validade jurídica. Foi criada para facilitar, reduzir custo e aumentar a segurança e compliance do processo de assinatura e workflow de documentos.

Documentação da API está disponível em <a href="http://clicksign.readme.io" target="_blank">http://clicksign.readme.io</a>.

Atualmente a biblioteca <a href="https://github.com/adrianocaldeira/clicksign-for-dotnet">Clicksign for .Net</a> está em acordo com a versão mais recente da Clicksign.

## Índice

- [Instalação](#instacao)
- [Configuração](#configuracao)
- [Utilização](#utilizacao)
	- [Recuperando lista de documentos](#utilizacao-lista-documento)
	- [Recuperando documento](#utilizacao-recupera-documento)
	- [Enviando um arquivo](#utilizacao-enviando-arquivo)
	- [Criando uma lista de assinatura](#utilizacao-criando-lista)
	- [Criando um Hook](#utilizacao-criando-hook)
	- [Enviando um arquivo e criando uma lista de assinatura em uma única chamada](#utilizacao-enviando-arquivo-lista-unica-chamada)
- [Release Notes](#release-notes)

## <a name="instacao"></a>Instalação

Para instalar, execute o seguinte comando no <a href="http://docs.nuget.org/docs/start-here/using-the-package-manager-console#Installing_a_Package" target="_blank">Package Manager Console</a>.

<img src="https://raw.githubusercontent.com/adrianocaldeira/clicksign-for-dotnet/master/nuget.png"/>

## <a name="configuracao"></a>Configuração

No arquivo de configuração do seu projeto adicione as linhas abaixo:

```xml
<appSettings>
  <add key="Clicksign-Host" value="URL DA CLICKSIGN API"/>
  <add key="Clicksign-Token" value="TOKEN"/>
</appSettings>
```

## <a name="utilizacao"></a>Utilização

####<a name="utilizacao-lista-documento"></a>Recuperando lista de documentos

Conforme a documentação http://clicksign.github.io/rest-api/#listagem-de-documentos, é possível obter uma listagem de todos os documentos da conta além de informações extras pertinentes ao andamento da lista de assinatura. A listagem retornarár todos os documentos na conta, sem a necessidade de parâmetros de paginação ou busca.

```csharp
var clicksign = new Clicksign();
var list = clicksign.List();

Console.Write(list.Count);
```

####<a name="utilizacao-recupera-documento"></a>Recuperando documento

Conforme a documentação http://clicksign.github.io/rest-api/#visualizacao-de-documento, é possível obter um documento da conta através da chave do documento, além de informações extras pertinentes ao andamento da lista de assinatura. 

```csharp
var clicksign = new Clicksign();
var document = clicksign.Get("1123-4567-89ab-cdef");

Console.Write(document.Key);
```

####<a name="utilizacao-enviando-arquivo"></a>Enviando um arquivo

Conforme a documentação http://clicksign.github.io/rest-api/#listagem-de-documentos, o processo de envio de um documento para a Clicksign contempla a criação de um arquivo de log contendo informações de upload, usuário, etc, anexado a uma cópia do documento "carimbada" com um número de série. Ao final do processo haverá 2 arquivos na Clicksign: documento original e arquivo de log. Enquanto o arquivo é processado a requisição não fica bloqueada. O status do documento será working enquanto o processo ocorre. Após concluído, o status será open.

```csharp
var clicksign = new Clicksign();

//Envio através do caminho do arquivo
var filePath = "c:\\doc.pdf";

clicksign.Upload(filePath);

Console.Write(clicksign.Document.Key);

//Envio através dos bytes de um arquivo
var fileBytes = File.ReadAllBytes(filePath);
var fileName = Path.GetFileName(filePath);

clicksign.Upload(fileBytes, fileName);

Console.Write(clicksign.Document.Key);
```

####<a name="utilizacao-criando-lista"></a>Criando uma lista de assinatura

Conforme a documentação http://clicksign.github.io/rest-api/#criacao-de-lista-de-assinatura, é possível criar uma lista de assinatura e enviá-la a outras pessoas em uma única ação. Para isso, é necessário que estejam presentes os campos que especificam o documento, os signatários, e a mensagem.

```csharp
var clicksign = new Clicksign();
var document = new Document { Key = "CHAVE DO ARQUIVO QUE FOI ENVIADO" };

//enviando apenas um signatário
clicksign.Signatories(document, new Signatory { Action = Sign, Email = "adriano.caldeira@gmail.com" });

//enviando mais de um signatário
clicksign.Signatories(document, new List<Signatory> {
	new Signatory { Action = Sign, Email = "adriano.caldeira@gmail.com" },
	new Signatory { Action = Sign, Email = "fkvillani@hotmail,com" }
});

Console.Write(clicksign.Document.Key);
```

####<a name="utilizacao-criando-hook"></a>Criando um Hook

Conforme a documentação http://clicksign.github.io/rest-api/#hooks, é possível que a Clicksign notifique outras aplições à respeito da alteração de estado de um determinado documento.

```csharp
var clicksign = new Clicksign();
var document = new Document { Key = "CHAVE DO ARQUIVO QUE FOI ENVIADO" };

var hook = clicksign.CreateHook(document, "https://www.linkedin.com/in/adrianocaldeira"});

Console.Write(hook.Id);
```

####<a name="utilizacao-enviando-arquivo-lista-unica-chamada"></a>Enviando um arquivo e criando uma lista de assinatura em uma única chamada

```csharp
var clicksign = new Clicksign();
var filePath = "c:\\doc.pdf";

var document = clicksign.Upload(filePath)
	.Signatories(new Signatory { Email = "adriano.caldeira@gmail.com", Action = SignatoryAction.Sign })
	.Document;
	
Console.Write(clicksign.Document.Key);	
```

##<a name="release-notes"></a>Release Notes

- Versão 1.0.0
	- Primeira versão lançada no NuGet.
- Versão 1.0.1
	- Versão da API no path da rota.
	- Exclusão da versão da API do Header da chamada.
- Versão 1.0.2
	- Correção de bugs.
- Versão 1.0.3
	- Implementação de log4net.
- Versão 1.0.4
	- Implementação do método Get referente a funcionalidade de visualizar documento, conforme documentação http://clicksign.github.io/rest-api/#visualizacao-de-documento.
	- Atualização de dependências.
	- Projeto para testar os métodos da API.
- Versão 1.0.5
	- Atualização de dependências.

