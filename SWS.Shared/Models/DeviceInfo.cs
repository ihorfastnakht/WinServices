using System;
using System.Diagnostics;

namespace SWS.Shared.Models
{
    public class DeviceInfo
    {
        #region Members

        public Guid Id { get; private set; }

        public int VendorId { get; private set; }

        public int ProductId { get; private set; }

        public string DeviceName { get; private set; }

        public DateTime? LastSyncTime { get; private set; }

        #endregion

        #region Constructor

        public DeviceInfo(Guid id, int vendorId, int productId, string deviceName)
        {
            Id = id;

            VendorId = vendorId <= 0 
                ? throw new ArgumentNullException(nameof(vendorId), "Invalid vendorId value") 
                : vendorId;

            ProductId = productId <= 0
                ? throw new ArgumentNullException(nameof(vendorId), "Invalid productId value")
                : productId;

            DeviceName = String.IsNullOrWhiteSpace(deviceName) 
                ? "Unnamed" 
                : deviceName;
        }

        #endregion

        #region Public methods

        public void SetLastSynchronizationTime()
        {
            this.LastSyncTime = DateTime.Now;
            Debug.WriteLine("Last sync time has been updated");
        }

        #endregion

        #region Override

        public override string ToString()
        {
            return $"{this.Id};{this.VendorId};{this.ProductId};{this.DeviceName}";
        }

        #endregion
    }
}
