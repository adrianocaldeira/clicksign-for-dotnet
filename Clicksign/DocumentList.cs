using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Clicksign
{
    /// <summary>
    /// Document list, more information visit <see cref="http://clicksign.github.io/rest-api-v2">Clicksign Rest API</see>
    /// </summary>
    public class DocumentList
    {
        /// <summary>
        /// Get or set started date
        /// </summary>
        [DeserializeAs(Name = "started_at")]
        public DateTime Started { get; set; }

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
        /// Get or set list of <see cref="DocumentSignature"/>
        /// </summary>
        public List<DocumentSignature> Signatures { get; set; }
    }
}