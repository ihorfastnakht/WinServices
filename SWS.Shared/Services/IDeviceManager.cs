using System.Threading.Tasks;

namespace SWS.Shared.Services
{
    using Models;

    public interface IDeviceManager
    {
        Task<bool> CheckDeviceRegistrationAsync(DeviceInfo deviceInfo);
        Task RegisterDeviceAsync(DeviceInfo deviceInfo);
    }
}
