using Functions.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Functions.Services
{
    public class SettingService: ISettingService
    {
        private readonly IConfiguration _configuration;
        public SettingService(IConfiguration configuration) => _configuration = configuration;

        public async Task<string> GetAsync(string name)
        {
            var val = _configuration[name] ?? string.Empty;
            return val;
        }
    }
}
