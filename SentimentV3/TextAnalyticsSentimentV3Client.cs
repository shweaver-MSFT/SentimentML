using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SentimentML.SentimentV3
{
    public class TextAnalyticsSentimentV3Client
    {
        private readonly string _textAnalyticsUrl;
        private readonly string _textAnalyticsKey;

        public TextAnalyticsSentimentV3Client(string textAnalyticsUrl, string textAnalyticsKey)
        {
            _textAnalyticsUrl = textAnalyticsUrl ?? throw new ArgumentNullException(textAnalyticsUrl);
            _textAnalyticsKey = textAnalyticsKey ?? throw new ArgumentNullException(textAnalyticsKey);
        }

        public async Task<SentimentV3Response> SentimentV3PreviewPredictAsync(TextAnalyticsBatchInput inputDocuments)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _textAnalyticsKey);

                var httpContent = new StringContent(JsonConvert.SerializeObject(inputDocuments), Encoding.UTF8, "application/json");

                var httpResponse = await httpClient.PostAsync(new Uri(_textAnalyticsUrl + "/v3.0-preview/sentiment"), httpContent);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(responseContent);

                if (!httpResponse.StatusCode.Equals(HttpStatusCode.OK) || httpResponse.Content == null)
                {
                    throw new Exception(responseContent);
                }

                return JsonConvert.DeserializeObject<SentimentV3Response>(responseContent, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            }
        }
    }
}