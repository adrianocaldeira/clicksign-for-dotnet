using System;
using RestSharp.Deserializers;

namespace Clicksign
{
    /// <summary>
    /// Document, more information visit <see cref="http://clicksign.github.io/rest-api-v2">Clicksign Rest API</see>
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Get or set id
        /// </summary>
        [DeserializeAs(Name = "archive_id")]
        public int Id { get; set; }

        /// <summary>
        /// Get or set key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Get or set name
        /// </summary>
        [DeserializeAs(Name = "original_name")]
        public string Name { get; set; }

        /// <summary>
        /// Get or set status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Get or set created date
        /// </summary>
        [DeserializeAs(Name = "created_at")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Get or set updated date
        /// </summary>
        [DeserializeAs(Name = "updated_at")]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Get or set user
        /// </summary>
        [DeserializeAs(Name = "user_key")]
        public string User { get; set; }

        /// <summary>
        /// Get or set document list
        /// </summary>
        public DocumentList List { get; set; }
    }
}