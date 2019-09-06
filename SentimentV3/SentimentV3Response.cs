using System.Collections.Generic;
using Newtonsoft.Json;

namespace SentimentML.SentimentV3
{
        public class SentimentV3Response
        {
            public IList<DocumentSentiment> Documents { get; set; }

            public IList<ErrorRecord> Errors { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public RequestStatistics Statistics { get; set; }
        }
}