using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Utils
{
    public class HttpHelper
    {
        public static HttpClient httpClient = new HttpClient();

        public static async Task<string> Execute<TReq, TRes>(TReq input, HttpMethod method, string contentType, string token, string url)
        {
            var message = new HttpRequestMessage();
            message.Method = method;
            message.Headers.Clear();
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-TOKEN", token);
            }
            message.RequestUri = new Uri(url);
            message.Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8,
                                    contentType);
            var response = await httpClient.SendAsync(message);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public static async Task<TRes> ExecuteGet<TRes>(string contentType, string token, string url)
        {
            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.Headers.Clear();
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-TOKEN", token);
            }
            message.RequestUri = new Uri(url);
            var response = await httpClient.SendAsync(message);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TRes>(responseString);
        }

        public static async Task<string> ExecuteGet(string contentType, string token, string url)
        {
            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.Headers.Clear();
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-TOKEN", token);
            }
            message.RequestUri = new Uri(url);
            var response = await httpClient.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
