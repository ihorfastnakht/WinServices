using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace SWS.UsbServiceListener
{
    [RunInstaller(true)]
    public partial class UsbServiceInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public UsbServiceInstaller()
        {
            InitializeComponent();

            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "UsbService";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
