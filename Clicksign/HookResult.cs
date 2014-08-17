using System;
using RestSharp.Deserializers;

namespace Clicksign
{
    /// <summary>
    /// Hook result, more information visit <see cref="http://clicksign.github.io/rest-api-v2/#hooks">Clicksign Rest API</see>
    /// </summary>
    public class HookResult
    {
        /// <summary>
        /// Get or set id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Get or set document id
        /// </summary>
        public int DocumentId { get; set; }
        /// <summary>
        /// Get or set url
        /// </summary>
        public string Url { get; set; }
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
    }
}