using System;
using System.Windows.Forms;
using LibUsbDotNet.DeviceNotify;

namespace TestClient
{
    using SWS.Shared.Models;
    using SWS.Shared.Services;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AttachUsbNotifier();

                while (true)
                    Application.DoEvents();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        #region Private members

        private static readonly IDeviceManager deviceManager = new DeviceManager();
        public static IDeviceNotifier usbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();

        #endregion

        #region Private methods

        private static void AttachUsbNotifier()
        {
            usbDeviceNotifier.OnDeviceNotify += OnDeviceNotifyEvent;
        }

        private static async void OnDeviceNotifyEvent(object sender, DeviceNotifyEventArgs e)
        {
            if (e.EventType == EventType.DeviceArrival)
            {
                var deviceInfo = new DeviceInfo(Guid.NewGuid(), e.Device.IdVendor, e.Device.IdProduct, e.Device.SymbolicName.FullName);

                var isRegistered = await deviceManager.CheckDeviceRegistrationAsync(deviceInfo);
                if (!isRegistered)
                {
                    await deviceManager.RegisterDeviceAsync(deviceInfo);
                }

                //Run sunchronization process
            }
            else
            {
                // Device has been removed
                // Add record some log file
            }

        }

        private static void DetachUsbNotifier()
        {
            usbDeviceNotifier.Enabled = false;
            usbDeviceNotifier.OnDeviceNotify -= OnDeviceNotifyEvent;
        }

        #endregion
    }
}
