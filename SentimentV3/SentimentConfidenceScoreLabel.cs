namespace SentimentML.SentimentV3
{
    public class SentimentConfidenceScoreLabel
    {
        public double Positive { get; set; }
        public double Neutral { get; set; }
        public double Negative { get; set; }

        public string PositivePercent => Positive.ToString("P");
        public string NeutralPercent => Neutral.ToString("P");
        public string NegativePercent => Negative.ToString("P");
    }
}