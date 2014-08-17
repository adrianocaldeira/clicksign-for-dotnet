using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using RestSharp;

namespace Clicksign
{
    /// <summary>
    /// Clicksign API, more information visit <see cref="http://clicksign.github.io/rest-api-v2">Clicksign Rest API</see>
    /// </summary>
    public class Clicksign
    {
        /// <summary>
        /// Initialize new instance of class <see cref="Clicksign"/>
        /// </summary>
        public Clicksign()
        {
            Host = ConfigurationManager.AppSettings["Clicksign-Host"];
            Token = ConfigurationManager.AppSettings["Clicksign-Token"];
        }

        /// <summary>
        /// Initialize new instance of class <see cref="Clicksign"/>
        /// </summary>
        /// <param name="token">Token</param>
        public Clicksign(string token)
            : this()
        {
            Token = token;
        }

        /// <summary>
        /// Initialize new instance of class <see cref="Clicksign"/>
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="token">Token</param>
        public Clicksign(string host, string token)
            : this(token)
        {
            Host = host;
        }

        /// <summary>
        /// Get Clicksign host
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Get Token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Get <see cref="Document"/>
        /// </summary>
        public Document Document { get; private set; }

        /// <summary>
        /// Upload file, more information visit <see cref="http://clicksign.github.io/rest-api-v2/#upload-de-documentos">Clicksign Rest API</see>
        /// </summary>
        /// <param name="file">File</param>
        /// <returns><see cref="Clicksign"/></returns>
        public Clicksign Upload(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file", "File path is empty.");

            if (!File.Exists(file))
                throw new FileNotFoundException(string.Format("File {0} not found.", file));

            return Upload(File.ReadAllBytes(file), Path.GetFileName(file));
        }

        /// <summary>
        /// Upload file, more information visit <see cref="http://clicksign.github.io/rest-api-v2/#upload-de-documentos">Clicksign Rest API</see>
        /// </summary>
        /// <param name="file">Bytes of file</param>
        /// <param name="fileName">File name</param>
        /// <returns><see cref="Clicksign"/></returns>
        public Clicksign Upload(byte[] file, string fileName)
        {
            if (file.Length.Equals(0))
                throw new ArgumentNullException("file", "File is empty.");

            if (string.IsNullOrEmpty(fileName))
                throw new FileNotFoundException("File name is null or empty.");

            var client = new RestClient(Host);
            var request = new RestRequest("documents", Method.POST);

            request.AddParameter("access_token", Token);
            request.AddHeader("Content-Type", "multipart/mixed; boundary=frontier");
            request.AddHeader("Accept", "application/vnd.clicksign.v1; application/json");
            request.AddFile("document[archive][original]", file, fileName);

            var response = client.Execute<Result>(request).Data;

            Document = response.Document;

            return this;
        }

        /// <summary>
        /// Create list of <see cref="Signatory"/>
        /// </summary>
        /// <example>
        ///     <see cref="http://clicksign.github.io/rest-api-v2/#criacao-de-lista-de-assinatura">Sample and documentation</see>
        /// </example>
        /// <param name="signatory"><see cref="Signatory"/></param>
        /// <returns><see cref="Clicksign"/></returns>
        public Clicksign Signatories(Signatory signatory)
        {
            return Signatories(Document, signatory);
        }

        /// <summary>
        /// Create list of <see cref="Signatory"/>
        /// </summary>
        /// <example>
        ///     <see cref="http://clicksign.github.io/rest-api-v2/#criacao-de-lista-de-assinatura">Sample and documentation</see>
        /// </example>        
        /// <param name="signatories">List of <see cref="Signatory"/></param>
        /// <returns><see cref="Clicksign"/></returns>
        public Clicksign Signatories(IList<Signatory> signatories)
        {
            return Signatories(Document, signatories);
        }

        /// <summary>
        /// Create list of <see cref="Signatory"/>
        /// </summary>
        /// <example>
        ///     <see cref="http://clicksign.github.io/rest-api-v2/#criacao-de-lista-de-assinatura">Sample and documentation</see>
        /// </example>  
        /// <param name="document"><see cref="Document"/></param>
        /// <param name="signatory"><see cref="Signatory"/></param>
        /// <returns><see cref="Clicksign"/></returns>
        public Clicksign Signatories(Document document, Signatory signatory)
        {
            return Signatories(document, new List<Signatory> { signatory });
        }

        /// <summary>
        /// Create list of <see cref="Signatory"/>
        /// </summary>
        /// <example>
        ///     <see cref="http://clicksign.github.io/rest-api-v2/#criacao-de-lista-de-assinatura">Sample and documentation</see>
        /// </example>  
        /// <param name="document"><see cref="Document"/></param>
        /// <param name="signatories">List of <see cref="Signatory"/></param>
        /// <returns><see cref="Clicksign"/></returns>
        public Clicksign Signatories(Document document, IList<Signatory> signatories)
        {
            if (document == null || string.IsNullOrEmpty(document.Key))
                throw new ArgumentNullException("document", "Document not informed or empty key");

            if (!signatories.Any())
                throw new ArgumentNullException("signatories", "Signatories is empty");

            var client = new RestClient(Host);
            var request = new RestRequest(string.Format("documents/{0}/list", document.Key), Method.POST);

            request.AddHeader("Accept", "application/vnd.clicksign.v1;application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("access_token", Token);

            foreach (var signatory in signatories)
            {
                request.AddParameter("signers[][email]", signatory.Email);
                request.AddParameter("signers[][act]", signatory.Action.ToString().ToLower());
            }

            var response = client.Execute<Result>(request);

            Document = response.Data.Document;

            return this;
        }

        /// <summary>
        /// List of <see cref="Document"/>, more information visit <see cref="http://clicksign.github.io/rest-api-v2/#listagem-de-documentos">Clicksign Rest API</see>
        /// </summary>
        /// <returns>List of <see cref="Document"/></returns>
        public List<Document> List()
        {
            var client = new RestClient(Host);
            var request = new RestRequest("documents", Method.GET);

            request.AddParameter("access_token", Token);
            request.AddHeader("Accept", "application/vnd.clicksign.v1; application/json");

            var response = client.Execute<List<Result>>(request);

            return response.Data.Select(result => result.Document).ToList();
        }

        /// <summary>
        /// Create hook, more information visit <see cref="http://clicksign.github.io/rest-api-v2/#hooks">Clicksign Rest API</see>
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns><see cref="HookResult"/></returns>
        public HookResult CreateHook(string url)
        {
            return CreateHook(Document, url);
        }

        /// <summary>
        /// Create hook, more information visit <see cref="http://clicksign.github.io/rest-api-v2/#hooks">Clicksign Rest API</see>
        /// </summary>
        /// <param name="document"><see cref="Document"/></param>
        /// <param name="url">Url</param>
        /// <returns><see cref="HookResult"/></returns>
        public HookResult CreateHook(Document document, string url)
        {
            if (document == null || string.IsNullOrEmpty(document.Key))
                throw new ArgumentNullException("document", "Document not informed or empty key");

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url", "Url is null or empty");

            var client = new RestClient(Host);
            var request = new RestRequest(string.Format("documents/{0}/hooks", document.Key), Method.POST);

            request.AddHeader("Accept", "application/vnd.clicksign.v1;application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("access_token", Token);
            request.AddParameter("url", url);

            return client.Execute<HookResult>(request).Data;
        }
    }
}
