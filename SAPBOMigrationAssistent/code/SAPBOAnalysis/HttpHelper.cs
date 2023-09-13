using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SAPBOAnalysis
{
    public class HttpHelper
    {
        public static HttpClient httpClient = new HttpClient();

        public async Task<TRes> Execute<TReq, TRes>(TReq input, HttpMethod method, string contentType, string token, string url)
        {
            var message = new HttpRequestMessage();
            message.Method = method;
            message.Headers.Clear();
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-SAP-LOGONTOKEN", token);
            }
            message.RequestUri = new Uri(url);
            message.Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8,
                                    contentType);
            var response = await httpClient.SendAsync(message);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TRes>(responseString);
        }

        public async Task<TRes> ExecuteGet<TRes>(string contentType, string token, string url)
        {
            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.Headers.Clear();
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-SAP-LOGONTOKEN", token);
            }
            message.RequestUri = new Uri(url);
            var response = await httpClient.SendAsync(message);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TRes>(responseString);
        }

        public async Task<string> ExecuteGet(string contentType, string token, string url)
        {
            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.Headers.Clear();
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            if (!string.IsNullOrEmpty(token))
            {
                message.Headers.Add("X-SAP-LOGONTOKEN", token);
            }
            message.RequestUri = new Uri(url);
            var response = await httpClient.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<TRes> ExecutePowerBI<TReq, TRes>(TReq input, HttpMethod method, string contentType, string token, string url)
        {
            var message = new HttpRequestMessage();
            message.Method = method;
            message.Headers.Clear();
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            message.RequestUri = new Uri(url);
            message.Content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8,
                                    contentType);
            var response = await httpClient.SendAsync(message);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TRes>(responseString);
        }
    }
}
