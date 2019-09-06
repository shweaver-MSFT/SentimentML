namespace SentimentML.SentimentV3
{
    public class SentenceSentiment
    {
        /// <summary>
        /// The predicted Sentiment for the sentence.
        /// </summary>
        public SentenceSentimentLabel Sentiment { get; set; }

        /// <summary>
        /// The sentiment confidence score for the sentence for all classes.
        /// </summary>
        public SentimentConfidenceScoreLabel SentenceScores { get; set; }

        /// <summary>
        /// The sentence offset from the start of the document.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// The sentence length as given by StringInfo's LengthInTextElements property.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The warnings generated for the sentence.
        /// </summary>
        public string[] Warnings { get; set; }

        /// <summary>
        /// Keeps a copy of the text used to generate this SentenceSentiment instance.
        /// </summary>
        public string OriginalText { get; private set; }

        /// <summary>
        /// Updates the OriginalText value by finding the substring via Offset and Length values.
        /// </summary>
        /// <param name="documentText"></param>
        public void SetOriginalText(string documentText)
        {
            OriginalText = documentText.Substring(Offset, Length);
        }
    }
}