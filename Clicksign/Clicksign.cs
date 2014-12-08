using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using log4net;
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
            Log = LogManager.GetLogger("Clicksign");
        }

        private ILog Log { get; set; } 

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
            var request = new RestRequest("v1/documents", Method.POST);

            request.AddParameter("access_token", Token);
            request.AddHeader("Content-Type", "multipart/mixed; boundary=frontier");
            request.AddHeader("Accept", "application/json");
            request.AddFile("document[archive][original]", file, fileName);

            Log.Debug(string.Format("Send file {0} with token {1}", fileName, Token));

            var response = Execute<Result>(client, request).Data;

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
            var request = new RestRequest(string.Format("v1/documents/{0}/list", document.Key), Method.POST);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("access_token", Token);
            request.AddParameter("skip_email", document.List.SkipEmail.ToString().ToLower());
            request.AddParameter("message", document.List.Message);

            Log.Debug(string.Format("Send list of Signatories with Token {0}, SkipEmail {1}, Message {2} and {3} signatories",
                Token, document.List.SkipEmail, document.List.Message, signatories.Count));

            foreach (var signatory in signatories)
            {
                var action = signatory.Action.ToString().ToLower();

                request.AddParameter("signers[][email]", signatory.Email);
                request.AddParameter("signers[][act]", action);

                Log.Debug(string.Format("Send Signatory Email {0} and Action {1} to list", signatory.Email, action));
            }
            
            var response = Execute<Result>(client, request);

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
            var request = new RestRequest("v1/documents", Method.GET);

            request.AddParameter("access_token", Token);
            request.AddHeader("Accept", "application/json");

            Log.Debug(string.Format("Get list of document with Token {0}", Token));
            
            var response = Execute<List<Result>>(client, request);
            var documents = response.Data.Select(result => result.Document).ToList();

            Log.Debug(string.Format("Get {0} documents of list", documents.Count));

            return documents;
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
            var request = new RestRequest(string.Format("v1/documents/{0}/hooks", document.Key), Method.POST);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("access_token", Token);
            request.AddParameter("url", url);

            Log.Debug(string.Format("Create hook of document with Token {0}, Document {1} and Url {2}", Token, document.Key, url));
            
            return Execute<HookResult>(client, request).Data;
        }

        /// <summary>
        /// Get <see cref="Document"/>, more information visit <see cref="http://clicksign.github.io/rest-api/#visualizacao-de-documento">Clicksign Rest API</see>
        /// </summary>
        /// <returns><see cref="Document"/></returns>
        public Document Get(string key)
        {
            var client = new RestClient(Host);
            var request = new RestRequest(string.Format("v1/documents/{0}", key), Method.GET);

            request.AddParameter("access_token", Token);
            request.AddHeader("Accept", "application/json");

            Log.Debug(string.Format("Get document with Token {0}", Token));

            var response = Execute<Result>(client, request);
            var document = response.Data.Document;

            if(document == null) Log.Debug("Document not found with key " + key);

            return document;
        }

        private IRestResponse<T> Execute<T>(RestClient client, IRestRequest request) where T : new()
        {
            try
            {
                var response = client.Execute<T>(request);

                Log.Debug(string.Format("Status Code {0}, Status Description {1} and Content {2}",
                    response.StatusCode, 
                    (string.IsNullOrEmpty(response.StatusDescription) ? "is empty" : response.StatusDescription),
                    (string.IsNullOrEmpty(response.Content) ? "is empty" : response.Content)));
                
                if (response.ErrorException != null)
                    throw response.ErrorException;

                if (!string.IsNullOrEmpty(response.ErrorMessage))
                    throw new Exception(response.ErrorMessage);
                
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Erro of execute ClickSign API", ex);
                throw;
            }
        }
    }
}
