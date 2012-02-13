using System;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;

namespace Wonga.QA.Framework.Svc
{
    public class SvcService
    {
        private ServiceController _controller;

        public SvcService(String service, String server)
        {
            _controller = new ServiceController(service, server);
        }

        public Boolean IsRunning()
        {
            Trace.WriteLine(String.Format("{0} is {1}", _controller.ServiceName, _controller.Status), GetType().FullName);
            return _controller.Status == ServiceControllerStatus.Running;
        }

        public String GetPath()
        {
            return RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, _controller.MachineName).OpenSubKey("SYSTEM").OpenSubKey("CurrentControlSet").OpenSubKey("Services").OpenSubKey(_controller.ServiceName).GetValue("ImagePath") as String;
        }
    }
}
