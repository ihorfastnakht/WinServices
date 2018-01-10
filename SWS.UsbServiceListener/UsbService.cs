using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using LibUsbDotNet.DeviceNotify;

namespace SWS.UsbServiceListener
{
    using SWS.Shared.Models;
    using SWS.Shared.Services.Interfaces;
    using SWS.Shared.Services.Implementation;
    using System.Threading;

    public partial class UsbService : ServiceBase
    {
        #region Private members

        private readonly IDeviceManager deviceManager;
        public IDeviceNotifier usbDeviceNotifier;

        #endregion

        #region Private methods

        private void AttachUsbNotifier()
        {
            usbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();
            usbDeviceNotifier.OnDeviceNotify += OnDeviceNotify;
        }

        private async void OnDeviceNotify(object sender, DeviceNotifyEventArgs e)
        {
            if (e.EventType == EventType.DeviceArrival)
            {
                var deviceInfo = new DeviceInfo(Guid.NewGuid(), e.Device.IdVendor, e.Device.IdProduct, e.Device.Name);

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
            deviceManager = new DeviceManager();
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
