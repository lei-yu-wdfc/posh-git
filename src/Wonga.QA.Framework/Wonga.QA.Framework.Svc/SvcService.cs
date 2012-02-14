using System;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Svc
{
    public class SvcService
    {
        private ServiceController _controller;

        public ServiceControllerStatus Status { get { _controller.Refresh(); return _controller.Status; } }

        public SvcService(String service, String server)
        {
            _controller = new ServiceController(service, server);
        }

        public Boolean IsRunning()
        {
            Trace.WriteLine(String.Format("{0} is {1}", _controller.ServiceName, _controller.Status), GetType().FullName);
            Do.While(() => Status == ServiceControllerStatus.StartPending);
            return _controller.Status == ServiceControllerStatus.Running;
        }

        public String GetPath()
        {
            return RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, _controller.MachineName).OpenSubKey("SYSTEM").OpenSubKey("CurrentControlSet").OpenSubKey("Services").OpenSubKey(_controller.ServiceName).GetValue("ImagePath") as String;
        }
    }
}
