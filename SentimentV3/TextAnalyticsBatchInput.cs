using System.Collections.Generic;

namespace SentimentML.SentimentV3
{
    public class TextAnalyticsBatchInput
    {
        public IList<TextAnalyticsInput> Documents { get; set; }
    }
}