using Dapr.Actors;

namespace MVC.Interfaces;
public interface IController : IActor
{
    Task RegisterDeviceIdsAsync(string[] deviceIds);
    Task<string[]> ListRegisteredDeviceIdsAsync();
    Task TriggerAlarmForAllDetectors();
}

public class ControllerData
{
    public string[] DeviceIds { get; set; } = default!;
}