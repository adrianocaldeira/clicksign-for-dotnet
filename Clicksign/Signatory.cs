namespace Clicksign
{
    /// <summary>
    /// Signatory, more information visit <see cref="http://clicksign.github.io/rest-api-v2">Clicksign Rest API</see>
    /// </summary>
    public class Signatory
    {
        /// <summary>
        /// Get or set <see cref="SignatoryAction"/>
        /// </summary>
        public SignatoryAction Action { get; set; }
        /// <summary>
        /// Get or set E-mail
        /// </summary>
        public string Email { get; set; }
    }
}