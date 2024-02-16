using DemoAPI.Model;
using Orleans;

namespace Required.Interface
{
    public interface IGrainUser : IGrainWithStringKey
    {
        Task<UIModel> GetFormsAsync();
    }

}
