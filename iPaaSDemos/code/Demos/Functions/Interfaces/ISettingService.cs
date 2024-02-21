using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Interfaces
{
    public interface ISettingService
    {
        Task<string> GetAsync(string name);
    }
}
