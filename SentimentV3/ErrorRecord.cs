namespace SentimentML.SentimentV3
{
    public class ErrorRecord
    {
        /// <summary>
        /// The input document unique identifier that this error refers to.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The actual error message.
        /// </summary>
        public string Message { get; set; }
    }
}