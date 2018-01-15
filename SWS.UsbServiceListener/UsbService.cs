using System;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

using LibUsbDotNet.DeviceNotify;

namespace SWS.UsbServiceListener
{
    using SWS.Shared.Services;
    using SWS.Shared.Models;
    using SWS.Shared.DataAccess;

    public partial class UsbService : ServiceBase
    {
        #region Private members

        private readonly IDeviceManager deviceManager = new DeviceManager();
        public IDeviceNotifier usbDeviceNotifier;

        #endregion

        #region Private methods

        private void AttachUsbNotifier()
        {
            usbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();
            usbDeviceNotifier.OnDeviceNotify += OnDeviceNotify;

            while(true)
                Application.DoEvents();
        }

        private async void OnDeviceNotify(object sender, DeviceNotifyEventArgs e)
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
                // Add record to log some log file
            }

        }

        private void DetachUsbNotifier()
        {
            usbDeviceNotifier.Enabled = false;
            usbDeviceNotifier.OnDeviceNotify -= OnDeviceNotify;
        }

        #endregion

        public UsbService()
        {
            InitializeComponent();
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            Thread loggerThread = new Thread(new ThreadStart(AttachUsbNotifier));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            DetachUsbNotifier();
            Thread.Sleep(1000);
        }
    }
}
