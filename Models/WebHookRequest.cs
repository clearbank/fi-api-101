namespace WebhooksReceiver.Models
{
    /// <summary>
    /// Webhook request
    /// </summary>
    public sealed class WebHookRequest
    {
        /// <summary>
        /// Gets or sets the type of the web hook (event).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the version corresponding to this web hook request.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the payload corresponding to this web hook request.
        /// </summary>
        public object Payload { get; set; }

        /// <summary>
        /// Unique webhook request identifier
        /// </summary>
        public int Nonce { get; set; }
    }
}
