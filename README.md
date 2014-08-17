# Clicksign for .Net

Biblioteca em .Net para consumo do serviços da Clicksign.

A Clicksign é uma solução online para enviar, guardar e assinar documentos, com validade jurídica. Foi criada para facilitar, reduzir custo e aumentar a segurança e compliance do processo de assinatura e workflow de documentos.

Documentação da API pode está disponível em <a href="https://github.com/clicksign/rest-api-v2" target="_blank">https://github.com/clicksign/rest-api-v2</a>.

Atuamente a biblioteca <a href="https://github.com/adrianocaldeira/clicksign-for-dotnet">Clicksign for .Net</a> está em acordo com a versão mais recente da Clicksign.

# Índice

- [Instalação](#instacao)
- [Configuração](#configuracao)
- [Utilização](#utilizacao)

# <a name="instacao"></a>Instalação

Para instalar, execute o seguinte comando no <a href="http://docs.nuget.org/docs/start-here/using-the-package-manager-console" target="_blank">Package Manager Console</a>.

<img src="https://raw.githubusercontent.com/adrianocaldeira/clicksign-for-dotnet/master/nuget.png"/>

# <a name="configuracao"></a>Configuração

No arquivo de configuração do seu projeto adicione as linhas abaixo:

```xml
<appSettings>
  <add key="Clicksign-Host" value="URL DA CLICKSIGN API"/>
  <add key="Clicksign-Token" value="TOKEN"/>
</appSettings>
```

# <a name="utilizacao"></a>Utilização

**Enviando um arquivo**

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




