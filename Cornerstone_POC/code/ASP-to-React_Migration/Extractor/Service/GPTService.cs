using Extractor.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Options;
using Extractor.Model;
using System.Runtime;
using System.Net.Http.Headers;

namespace Extractor.Service
{
    public class GPTService
    {

        //private static readonly HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(300) };
        private readonly string apiKey;
        private readonly string apiUrl;

        public GPTService(IOptions<AppSettings> appSettings)
        {
            var settings = appSettings.Value;
            apiKey = settings.GptApiKey;
            apiUrl = settings.GptUrl;
        }

        public async Task<(string Message, string Id)> GetAiResponse(string prompt, string systemPrompt, string model, bool logResponse)
        {
            var requestBody = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = prompt }
                },
                temperature = 0
            };

            //var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            //var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            //if (!client.DefaultRequestHeaders.Contains("Authorization"))
            //{
            //    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            //}

            HttpHelper.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var json = JsonConvert.SerializeObject(requestBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpHelper.httpClient.PostAsync(apiUrl, data);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                var parsedResponse = (JObject)JsonConvert.DeserializeObject(responseBody);

                var message = parsedResponse["choices"][0]["message"]["content"].ToString();
                var id = parsedResponse["id"]?.ToString();
                var logPrompt = "\n\nSystem Prompt = " + systemPrompt + "\n\n\nUser Prompt = " + prompt;
                var logMessage = logPrompt + prompt + "\n\n\nAssistant Response = " + message;
                if (logResponse)
                {
                    Console.WriteLine("Response:");
                    Console.WriteLine(message);

                }
                Logger.LogToFile(logPrompt);
                return (Message: message, Id: id);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                Console.WriteLine(responseBody);
                var errorMessage = $"Error: {response.StatusCode}. Response Body: {responseBody}";
                Logger.LogToFile(errorMessage);
                return (Message: errorMessage, Id: string.Empty);
            }
        }

        public async Task<(string Message, string Id)> GetAiResponseForImage(string prompt, string systemPrompt, string model, bool logResponse, string imagePath)
        {
            string base64Image = EncodeImage(imagePath);
            var requestBody = new
            {
                model = "gpt-4o",
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = prompt },
                            new { type = "image_url", image_url = new
                                    {
                                        url = $"data:image/png;base64,{base64Image}"
                                    }
                            }
                        }
                    }
                },
                temperature = 0
            };

            //var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            //var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            //if (!client.DefaultRequestHeaders.Contains("Authorization"))
            //{
            //    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            //}

            HttpHelper.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var json = JsonConvert.SerializeObject(requestBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpHelper.httpClient.PostAsync(apiUrl, data);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var parsedResponse = (JObject)JsonConvert.DeserializeObject(responseBody);
                var message = parsedResponse["choices"][0]["message"]["content"].ToString();
                var id = parsedResponse["id"]?.ToString();
                var logPrompt = "\n\nSystem Prompt = " + systemPrompt + "\n\n\nUser Prompt = " + prompt;
                var logMessage = logPrompt + prompt + "\n\n\nAssistant Response = " + message;
                if (logResponse)
                {
                    Console.WriteLine($"Response: {message}");
                }
                Logger.LogToFile(logPrompt);
                return (Message: message, Id: id);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}, {responseBody}");
                var errorMessage = $"Error: {response.StatusCode}. Response Body: {responseBody}";
                Logger.LogToFile(errorMessage);
                return (Message: errorMessage, Id: string.Empty);
            }
        }

        private string EncodeImage(string imagePath)
        {
            byte[] imageArray = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageArray);
        }
    }
}
