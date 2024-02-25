using Functions.Interfaces;
using Functions.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Services
{
    public class LogicAppService : ILogicAppService
    {
        private readonly HttpClient _client;
        private readonly ISettingService _settingService;
        public LogicAppService(ISettingService settingService, HttpClient httpClient)
        {
            _settingService = settingService;
            _client = httpClient;
        }

        public async Task Send(OrderModel order)
        {
            var requestBodyJson = JsonConvert.SerializeObject(order);
            StringContent requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
            var url = $"https://{await _settingService.GetAsync(SettingPropertyNames.LogicAppsServerUrl)}/{await _settingService.GetAsync(SettingPropertyNames.ReceiveOrderWorkflow)}/paths/invoke";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Ocp-Apim-Subscription-Key", await _settingService.GetAsync(SettingPropertyNames.SubKey));
            request.Content = requestContent;
            var response = await _client.SendAsync(request);
        }

        public async Task Send(OrderModel order, string url)
        {
            var requestBodyJson = JsonConvert.SerializeObject(order);
            StringContent requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = requestContent;
            var response = await _client.SendAsync(request);
        }
    }
}
