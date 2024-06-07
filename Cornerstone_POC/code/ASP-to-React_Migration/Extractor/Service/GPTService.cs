﻿using Extractor.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Service
{
    public class GPTService
    {

        private static readonly HttpClient client = new HttpClient();
        private readonly string apiKey;
        private readonly string apiUrl;

        public GPTService()
        {
            apiKey = Configuration.GptApiKey;
            apiUrl = Configuration.GptUrl;
        }

        public async Task<(string Message, string Id)> GetAiResponse(string prompt, string systemPrompt, string model, bool logResponse)
        {

            Logger.Log("Getting AI Response");
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

            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            if (!client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            }

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                var parsedResponse = (JObject)JsonConvert.DeserializeObject(responseBody);

                var message = parsedResponse["choices"][0]["message"]["content"].ToString();
                var id = parsedResponse["id"]?.ToString();
                var logMessage = "\n\nSystem Prompt = " + systemPrompt + "\n\n\nUser Prompt = " + prompt + "\n\n\nAssistant Response = " + message;
                if (logResponse)
                {
                    Console.WriteLine("Response:");
                    Console.WriteLine(message);

                }
                return (Message: message, Id: id);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                Console.WriteLine(responseBody);
                var errorMessage = $"Error: {response.StatusCode}. Response Body: {responseBody}";
                return (Message: errorMessage, Id: string.Empty);
            }
        }

        public async Task<(string Message, string Id)> GetAiResponseForImage(string prompt, string systemPrompt, string model, bool logResponse ,string imagePath)
        {
            Logger.Log("Reading Image");
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

            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            if (!client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            }

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                var parsedResponse = (JObject)JsonConvert.DeserializeObject(responseBody);

                var message = parsedResponse["choices"][0]["message"]["content"].ToString();
                var id = parsedResponse["id"]?.ToString();
                var logMessage = "\n\nSystem Prompt = " + systemPrompt + "\n\n\nUser Prompt = " + prompt + "\n\n\nAssistant Response = " + message;
                if (logResponse)
                {
                    Console.WriteLine("Response:");
                    Console.WriteLine(message);

                }
                return (Message: message, Id: id);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                Console.WriteLine(responseBody);
                var errorMessage = $"Error: {response.StatusCode}. Response Body: {responseBody}";
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
