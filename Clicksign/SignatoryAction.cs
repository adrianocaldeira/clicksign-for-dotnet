namespace Clicksign
{
    /// <summary>
    /// Signatory action, more information visit <see cref="http://clicksign.github.io/rest-api-v2">Clicksign Rest API</see>
    /// </summary>
    public enum SignatoryAction
    {
        /// <summary>
        /// Sign
        /// </summary>
        Sign,
        /// <summary>
        /// Approve
        /// </summary>
        Approve,
        /// <summary>
        /// Acknowledge
        /// </summary>
        Acknowledge,
        /// <summary>
        /// Witness
        /// </summary>
        Witness,
        /// <summary>
        /// Intervenient
        /// </summary>
        Intervenient,
        /// <summary>
        /// Party
        /// </summary>
        Party,
        /// <summary>
        /// Receipt
        /// </summary>
        Receipt
    }
}