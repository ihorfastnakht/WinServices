using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
    
namespace SWS.Shared.Services
{
    using Models;
    using DataAccess;

    public class DeviceManager : IDeviceManager
    {
        #region Members

        private readonly DevicesStorage deviceStorage = new DevicesStorage();
        
        #endregion

        #region Constructor
        #endregion

        #region IDeviceManager implementation

        public async Task<bool> CheckDeviceRegistrationAsync(DeviceInfo deviceInfo)
        {
            try
            {
                if (deviceInfo == null)
                {
                    throw new ArgumentNullException(nameof(deviceInfo));
                }
                var entities = await deviceStorage.GetEntitiesAsync();
                if (entities != null && entities.Count() > 0)
                {
                    return entities.FirstOrDefault(di => di.ProductId == deviceInfo.ProductId 
                                && di.VendorId == deviceInfo.VendorId) != null;
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e}");
                throw;
            }
        }

        public async Task RegisterDeviceAsync(DeviceInfo deviceInfo)
        {
            try
            {
                if (deviceInfo == null)
                {
                    throw new ArgumentNullException(nameof(deviceInfo));
                }

                await deviceStorage.SaveEntityAsync(deviceInfo);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e}");
                throw;
            }
        }

        #endregion
    }
}
