using System;
using RestSharp.Deserializers;

namespace Clicksign
{
    /// <summary>
    /// Document signature, more information visit <see cref="http://clicksign.github.io/rest-api-v2">Clicksign Rest API</see>
    /// </summary>
    public class DocumentSignature
    {
        /// <summary>
        /// Get o set name
        /// </summary>
        [DeserializeAs(Name = "display_name")]
        public string Name { get; set; }

        /// <summary>
        /// Get or set title
        /// </summary>
        [DeserializeAs(Name = "title")]
        public string Title { get; set; }
        /// <summary>
        /// Get or set company
        /// </summary>
        [DeserializeAs(Name = "company_name")]
        public string Company { get; set; }

        /// <summary>
        /// Get or set key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Get or set action
        /// </summary>
        [DeserializeAs(Name = "Act")]
        public SignatoryAction Action { get; set; }

        /// <summary>
        /// Get or set decision
        /// </summary>
        public string Decision { get; set; }

        /// <summary>
        /// Get or set ip address
        /// </summary>
        [DeserializeAs(Name = "address")]
        public string Ip { get; set; }

        /// <summary>
        /// Get or set e-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get or set created date
        /// </summary>
        [DeserializeAs(Name = "created_at")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Get or set signed date
        /// </summary>
        [DeserializeAs(Name = "signed_at")]
        public DateTime Signed { get; set; }

        /// <summary>
        /// Get or set updated date
        /// </summary>
        [DeserializeAs(Name = "updated_at")]
        public DateTime Updated { get; set; }
    }
}