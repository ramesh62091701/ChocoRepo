using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaDemo2.Interface
{
    public interface INotification
    {
        Task AddMessage(string quantity);

        Task<List<string>> GetMessage();

        Task ClearMessage();
    }
}
