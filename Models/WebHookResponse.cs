namespace WebhooksReceiver.Models
{
    /// <summary>
    /// Webhook response
    /// </summary>
    public sealed class WebHookResponse
    {
        /// <summary>
        /// Unique webhook request identifier
        /// </summary>
        public int Nonce { get; set; }
    }
}
