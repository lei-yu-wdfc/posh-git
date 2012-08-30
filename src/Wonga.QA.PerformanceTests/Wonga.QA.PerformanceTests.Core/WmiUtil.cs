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
        /// Returns All Wonga services (Wonga*)
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public List<String> GetAllWongaServices(ManagementScope scope)
        {
            var services = new List<String>();
            try
            {
                var query = new ObjectQuery(
                   "SELECT * FROM Win32_Service");

                var searcher =
                    new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    var service = queryObj["Caption"].ToString();

                    if (service.ToLower().StartsWith("wonga"))
                    {
                        services.Add(service);
                    }
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }
            return services;
        } 

        /// <summary>
        /// Stops All Wonga services
        /// </summary>
        /// <param name="scope"></param>
        public void StopAllWongaServices(ManagementScope scope)
        {
            var services = GetAllWongaServices(scope);

            foreach (var service in services)
            {
                StopService(scope, service);
            }
        }

        /// <summary>
        /// Start All Wonga services
        /// </summary>
        /// <param name="scope"></param>
        public void StartAllWongaServices(ManagementScope scope)
        {
            var services = GetAllWongaServices(scope);

            foreach (var service in services)
            {
                StartService(scope, service);
            }
        }

        /// <summary>
        /// Stop a service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean StopService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "StopService");
        }

        /// <summary>
        /// Start a service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean StartService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "StartService");
        }

        /// <summary>
        /// Pause a service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean PauseService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "PauseService");
        }

        /// <summary>
        /// Delete a service
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Boolean DeleteService(ManagementScope scope, string serviceName)
        {
            return InvokeWmiServiceMethod(scope, serviceName, "delete");
        }

        /// <summary>
        /// Resume a service
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
