using System.ServiceProcess;

namespace SWS.UsbServiceListener
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new UsbService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
