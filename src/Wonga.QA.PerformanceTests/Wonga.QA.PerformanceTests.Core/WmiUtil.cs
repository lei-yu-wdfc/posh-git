using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using MbUnit.Framework;

namespace Wonga.QA.PerformanceTests.Core
{
    public class WmiUtil
    {
        /// <summary>
        /// Connect to remote host
        /// </summary>
        /// <param name="host"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mgmtPath"></param>
        /// <returns></returns>
        public ManagementScope EstablishConnection(string host, string username, string password, string mgmtPath)
        {
            if (host.Equals("localhost") || host.Equals("127.0.0.1"))
                return null;

            if (string.IsNullOrEmpty(mgmtPath))
                mgmtPath = @"\root\CIMV2";

            mgmtPath = @"\\" + host + mgmtPath;
            var connection = new ConnectionOptions
                                 {
                                     Username = username, 
                                     Password = password, 
                                     Authority = "ntlmdomain:DOMAIN"
                                 };

            var scope = new ManagementScope(mgmtPath, connection);

            try
            {
                scope.Connect();
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot connect to the host " + host);
                Console.WriteLine("Exception Message: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return scope;
        }

        /// <summary>
        /// Invoke the method given in the remote machine using scope
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public Boolean InvokeWmiServiceMethod(ManagementScope scope, string serviceName, string method)
        {
            string objPath = string.Format("Win32_Service.Name='{0}'", serviceName);
            using (var service = new ManagementObject(new ManagementPath(objPath)))
            {
                try
                {
                    service.InvokeMethod(method, null, null);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower().Trim() == "not found" || ex.GetHashCode() == 41149443)
                        return false;
                    throw;
                }
            }
        }

        /// <summary>
        /// Stop the service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean StopService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "StopService");
        }

        /// <summary>
        /// Start the service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean StartService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "StartService");
        }

        /// <summary>
        /// Pause the service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean PauseService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "PauseService");
        }

        /// <summary>
        /// Delete the service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean DeleteService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "delete");
        }

        /// <summary>
        /// Resume the service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean ResumeService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "ResumeService");
        }
    }
}
