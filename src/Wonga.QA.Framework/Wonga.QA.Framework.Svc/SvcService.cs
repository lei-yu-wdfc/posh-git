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
            Do.While(() => Status == ServiceControllerStatus.StartPending);
            Trace.WriteLine(String.Format("{0} is {1}", _controller.ServiceName, _controller.Status), GetType().FullName);
            return _controller.Status == ServiceControllerStatus.Running;
        }

        public String GetPath()
        {
            return RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, _controller.MachineName).OpenSubKey("SYSTEM").OpenSubKey("CurrentControlSet").OpenSubKey("Services").OpenSubKey(_controller.ServiceName).GetValue("ImagePath") as String;
        }

        public SvcService Restart()
        {
            if (Status != ServiceControllerStatus.Stopped)
            {
                _controller.Stop();
                _controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(1));
            }
            _controller.Start();
            Do.Until(IsRunning, TimeSpan.FromMinutes(1));
            return this;
        }
    }
}
