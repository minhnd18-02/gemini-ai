using System.Text;
using System;
using GenAIWithGemini.Model;
using GenAIWithGemini.Model.ContentResponse;
using Newtonsoft.Json;

namespace GenAIWithGemini.Client
{
    public class GeminiApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public GeminiApiClient(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }
        public async Task<string> CheckTextAsync(string prompt)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";
            var request = new ContentRequest
            {
                contents = new[]
                {
                    new Model.Content
                    {
                        parts = new[]
                        {
                            new Model.Part
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };
            string jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                // You can deserialize jsonResponse if needed
                var geminiResponse = JsonConvert.DeserializeObject<ContentResponse>(jsonResponse);
                return geminiResponse.Candidates[0].Content.Parts[0].Text;
            }
            else
            {
                throw new Exception("Error communicating with Gemini API.");
            }
        }

        public async Task<string> AnalyzeImageAsync(string base64Image)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-vision:generateContent?key={_apiKey}";

            var request = new
            {
                contents = new[]
                {
            new
            {
                parts = new object[]
                {
                    new { text = "Hãy kiểm tra xem hình ảnh này có chứa nội dung nhạy cảm không. Nếu có, hãy trả lời 'Có', nếu không, hãy trả lời 'Không'." },
                    new { inlineData = new { mimeType = "image/png", data = base64Image } }
                }
            }
        }
            };

            string jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API request failed: {response.ReasonPhrase}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<ContentResponse>(jsonResponse);

            return geminiResponse.Candidates[0].Content.Parts[0].Text;
        }
    }
}
