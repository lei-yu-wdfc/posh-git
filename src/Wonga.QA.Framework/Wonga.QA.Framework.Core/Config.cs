﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;

namespace Wonga.QA.Framework.Core
{
    public enum AUT { Uk, Za, Ca, Wb }
    public enum SUT { Dev, WIP, UAT, RC, WIPRelease, RCRelease }

    public static class Config
    {
        public static SUT SUT { get; set; }
        public static AUT AUT { get; set; }

        public static ApiConfig Api { get; set; }
        public static SvcConfig Svc { get; set; }
        public static MsmqConfig Msmq { get; set; }
        public static DbConfig Db { get; set; }
        public static UiConfig Ui { get; set; }
        public static bool ProxyMode
        {
            get
            {
                var proxyModeVal = ReadEnvVar("ProxyMode");
                return !string.IsNullOrEmpty(proxyModeVal)  &&
                       proxyModeVal.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        static Config()
        {
            SUT = GetValue<SUT>();
            AUT = GetValue<AUT>();

            switch (SUT)
            {
                case SUT.Dev:
                    Api = new ApiConfig("localhost");
                    Svc = new SvcConfig(".");
                    Msmq = new MsmqConfig(".");
                    Db = new DbConfig(".");
                    Ui = new UiConfig("localhost");
                    break;
                case SUT.WIP:
                    Api = new ApiConfig(String.Format("wip.api.{0}.wonga.com", AUT));
                    Svc =
                        AUT == AUT.Uk ? new SvcConfig("WIP2") :
                        AUT == AUT.Za ? new SvcConfig("WIP4") :
                        AUT == AUT.Ca ? new SvcConfig("WIP6") :
                        AUT == AUT.Wb ? new SvcConfig("WIP8") : Throw<SvcConfig>();
                    Msmq =
                        AUT == AUT.Uk ? new MsmqConfig("WIP2") :
                        AUT == AUT.Za ? new MsmqConfig("WIP4") :
                        AUT == AUT.Ca ? new MsmqConfig("WIP6") :
                        AUT == AUT.Wb ? new MsmqConfig("WIP8") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Uk ? new DbConfig(Connections.GetDbConn("WIP2", ProxyMode)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("WIP4", ProxyMode)) :
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("WIP6", ProxyMode)) :
                        AUT == AUT.Wb ? new DbConfig(Connections.GetDbConn("WIP8", ProxyMode)) : Throw<DbConfig>();
                    Ui = new UiConfig(String.Format("wip.{0}.wonga.com", AUT));
                    break;
                case SUT.WIPRelease:
                    Api = new ApiConfig(String.Format("wip.release.api.{0}.wonga.com", AUT));
                    Svc =
                        AUT == AUT.Ca ? new SvcConfig("ca-rel-wip-app") :
                        AUT == AUT.Za ? new SvcConfig("za-rel-wip-app") : Throw<SvcConfig>();
                    Msmq =
                        AUT == AUT.Ca ? new MsmqConfig("ca-rel-wip-app") :
                        AUT == AUT.Za ? new MsmqConfig("za-rel-wip-app") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("ca-rel-wip-app", ProxyMode)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("za-rel-wip-app", ProxyMode)) : Throw<DbConfig>();
                    Ui = new UiConfig(String.Format("wip.release.{0}.wonga.com", AUT));
                    break;
                case SUT.UAT:
                    Api = new ApiConfig(String.Format("uat.api.{0}.wonga.com", AUT));
                    Svc =
                        AUT == AUT.Uk ? new SvcConfig("UAT2") :
                        AUT == AUT.Za ? new SvcConfig("UAT4") :
                        AUT == AUT.Ca ? new SvcConfig("UAT6") :
                        AUT == AUT.Wb ? new SvcConfig("UAT8") : Throw<SvcConfig>();
                    Msmq =
                        AUT == AUT.Uk ? new MsmqConfig("UAT2") :
                        AUT == AUT.Za ? new MsmqConfig("UAT4") :
                        AUT == AUT.Ca ? new MsmqConfig("UAT6") :
                        AUT == AUT.Wb ? new MsmqConfig("UAT8") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Uk ? new DbConfig(Connections.GetDbConn("UAT2", ProxyMode)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("UAT4", ProxyMode)) :
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("UAT6", ProxyMode)) :
                        AUT == AUT.Wb ? new DbConfig(Connections.GetDbConn("UAT8", ProxyMode)) : Throw<DbConfig>();
                    Ui = new UiConfig(String.Format("uat.{0}.wonga.com", AUT));
                    break;
                case SUT.RC:
                    Api = new ApiConfig(String.Format("rc.api.{0}.wonga.com", AUT));
                    Svc =
                        AUT == AUT.Uk ? new SvcConfig("RC2") :
                        AUT == AUT.Za ? new SvcConfig("RC4") :
                        AUT == AUT.Ca ? new SvcConfig("RC6") :
                        AUT == AUT.Wb ? new SvcConfig("RC9", "RC10") : Throw<SvcConfig>();
                    Msmq =
                        AUT == AUT.Uk ? new MsmqConfig("RC2") :
                        AUT == AUT.Za ? new MsmqConfig("RC4") :
                        AUT == AUT.Ca ? new MsmqConfig("RC6") :
                        AUT == AUT.Wb ? new MsmqConfig("RC9", "RC10") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Uk ? new DbConfig(Connections.GetDbConn("RC2", ProxyMode)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("RC4", ProxyMode)) :
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("RC6", ProxyMode)) :
                        AUT == AUT.Wb ? new DbConfig(Connections.GetDbConn("RC8", ProxyMode)) : Throw<DbConfig>();
                    Ui = new UiConfig(String.Format("rc.{0}.wonga.com", AUT));
                    break;
                case SUT.RCRelease:
                    Api = new ApiConfig(String.Format("rc.release.api.{0}.wonga.com", AUT));
                    Svc =
                        AUT == AUT.Ca ? new SvcConfig("ca-rel-rc-app") :
                        AUT == AUT.Za ? new SvcConfig("za-rel-rc-app") : Throw<SvcConfig>();
                    Msmq =
                        AUT == AUT.Ca ? new MsmqConfig("ca-rel-rc-app") :
                        AUT == AUT.Za ? new MsmqConfig("za-rel-rc-app") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("ca-rel-rc-app", ProxyMode)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("za-rel-rc-app", ProxyMode)) : Throw<DbConfig>();
                    Ui = new UiConfig(String.Format("rc.release.{0}.wonga.com", AUT));
                    break;
                default:
                    throw new NotImplementedException();
            }

            Trace.WriteLine(SUT, typeof(Config).FullName);
            Trace.WriteLine(AUT, typeof(Config).FullName);
            /*foreach (Object config in new Object[] { Api, Msmq, Db })
                foreach (PropertyInfo property in config.GetType().GetProperties())
                    Trace.WriteLine(String.Format("{0} = {1}", property.Name, property.GetValue(config, null)), config.GetType().FullName);*/
        }

        public static T Throw<T>()
        {
            throw new NotImplementedException(typeof(T).FullName);
        }

        private static T GetValue<T>()
        {
            return (T)Enum.Parse(typeof(T), ReadEnvVar(typeof(T).Name), true);
        }

        private static String ReadEnvVar(string name)
        {
            try
            {
                return (String)Registry.CurrentUser.OpenSubKey("Environment").GetValue(name);
            }
            catch
            {
                return null;
            }
        }

        public class ApiConfig
        {
            public Uri Commands { get; set; }
            public Uri Queries { get; set; }

            public ApiConfig(String host)
            {
                Uri uri = new UriBuilder { Host = host }.Uri;
                Commands = new Uri(uri, "commands");
                Queries = new Uri(uri, "queries");
            }
        }

        public class SvcConfig
        {
            public KeyValuePair<String, String> Ops { get; set; }
            public KeyValuePair<String, String> Comms { get; set; }
            public KeyValuePair<String, String> Payments { get; set; }
            public KeyValuePair<String, String> Risk { get; set; }
            public KeyValuePair<String, String> Marketing { get; set; }
            public KeyValuePair<String, String> Bi { get; set; }

            public KeyValuePair<String, String> BankGateway { get; set; }
            public KeyValuePair<String, String> Blacklist { get; set; }
            public KeyValuePair<String, String> BottomLine { get; set; }
            public KeyValuePair<String, String> CallReport { get; set; }
            public KeyValuePair<String, String> CallValidate { get; set; }
            public KeyValuePair<String, String> CardPayment { get; set; }
            public KeyValuePair<String, String> ColdStorage { get; set; }
            public KeyValuePair<String, String> ContactManagement { get; set; }
            public KeyValuePair<String, String> DocumentGeneration { get; set; }
            public KeyValuePair<String, String> Email { get; set; }
            public KeyValuePair<String, String> Equifax { get; set; }
            public KeyValuePair<String, String> Experian { get; set; }
            public KeyValuePair<String, String> ExperianBulk { get; set; }
            public KeyValuePair<String, String> FileStorage { get; set; }
            public KeyValuePair<String, String> Graydon { get; set; }
            public KeyValuePair<String, String> Hpi { get; set; }
            public KeyValuePair<String, String> Hsbc { get; set; }
            public KeyValuePair<String, String> Hyphen { get; set; }
            public KeyValuePair<String, String> Iovation { get; set; }
            public KeyValuePair<String, String> Salesforce { get; set; }
            public KeyValuePair<String, String> Scheduler { get; set; }
            public KeyValuePair<String, String> Scotia { get; set; }
            public KeyValuePair<String, String> Sms { get; set; }
            public KeyValuePair<String, String> TimeoutManager { get; set; }
            public KeyValuePair<String, String> TimeZone { get; set; }
            public KeyValuePair<String, String> TransUnion { get; set; }
            public KeyValuePair<String, String> Uru { get; set; }
            public KeyValuePair<String, String> WongaPay { get; set; }

            public SvcConfig(String server) : this(server, server) { }

            public SvcConfig(String service, String component)
            {
                Ops = new KeyValuePair<String, String>("Wonga.Ops.Handlers", service);
                Comms = new KeyValuePair<String, String>("Wonga.Comms.Handlers", service);
                Payments = new KeyValuePair<String, String>("Wonga.Payments.Handlers", service);
                Risk = new KeyValuePair<String, String>("Wonga.Risk.Handlers", service);
                Marketing = new KeyValuePair<String, String>("Wonga.Marketing.Handlers", service);
                Bi = new KeyValuePair<String, String>("Wonga.Bi.Handlers", service);

                BankGateway = new KeyValuePair<String, String>("Wonga.BankGateway.Handlers", component);
                Blacklist = new KeyValuePair<String, String>("Wonga.BlackList.Handlers", component);
                BottomLine = new KeyValuePair<String, String>("Wonga.BankGateway.Bottomline.Handlers", component);
                CallReport = new KeyValuePair<String, String>("Wonga.CallReport.Handlers", component);
                CallValidate = new KeyValuePair<String, String>("Wonga.CallValidate.Handlers", component);
                CardPayment = new KeyValuePair<String, String>("Wonga.CardPayment.Handlers", component);
                ColdStorage = new KeyValuePair<String, String>("Wonga.ColdStorage.Handlers", component);
                ContactManagement = new KeyValuePair<String, String>("Wonga.Comms.ContactManagement.Handlers", component);
                DocumentGeneration = new KeyValuePair<String, String>("Wonga.Comms.DocumentGeneration.Handlers", component);
                Email = new KeyValuePair<String, String>("Wonga.Email.Handlers", component);
                Equifax = new KeyValuePair<String, String>("Wonga.Equifax.Handlers", component);
                Experian = new KeyValuePair<String, String>("Wonga.Experian.Handlers", component);
                ExperianBulk = new KeyValuePair<String, String>("Wonga.ExperianBulk.Handlers", component);
                FileStorage = new KeyValuePair<String, String>("Wonga.FileStorage.Handlers", component);
                Graydon = new KeyValuePair<String, String>("Wonga.Graydon.Handlers", component);
                Hpi = new KeyValuePair<String, String>("Wonga.HPI.Handlers", component);
                Hsbc = new KeyValuePair<String, String>("Wonga.BankGateway.HSBC.Handlers", component);
                Hyphen = new KeyValuePair<String, String>("Wonga.BankGateway.Hyphen.Handlers", component);
                Iovation = new KeyValuePair<String, String>("Wonga.Iovation.Handlers", component);
                Salesforce = new KeyValuePair<String, String>("Wonga.Salesforce.Handlers", component);
                Scheduler = new KeyValuePair<String, String>("Wonga.Scheduler.Handlers", component);
                Scotia = new KeyValuePair<String, String>("Wonga.BankGateway.Scotia.Handlers", component);
                Sms = new KeyValuePair<String, String>("Wonga.Sms.Handlers", component);
                TimeoutManager = new KeyValuePair<String, String>("Wonga.TimeoutManager.Handlers", component);
                TimeZone = new KeyValuePair<String, String>("Wonga.TimeZone.Handlers", component);
                TransUnion = new KeyValuePair<String, String>("Wonga.TransUnion.Handlers", component);
                Uru = new KeyValuePair<String, String>("Wonga.URU.Handlers", component);
                WongaPay = new KeyValuePair<String, String>("Wonga.WongaPay.Handlers", component);
            }
        }

        public class MsmqConfig
        {
            public String Ops { get; set; }
            public String Comms { get; set; }
            public String Payments { get; set; }
            public String Risk { get; set; }
            public String Marketing { get; set; }
            public String Bi { get; set; }

            public String BankGateway { get; set; }
            public String BottomLine { get; set; }
            public String Hsbc { get; set; }
            public String Hyphen { get; set; }
            public String Scotia { get; set; }
            public String Blacklist { get; set; }
            public String CallReport { get; set; }
            public String CallValidate { get; set; }
            public String CardPayment { get; set; }
            public String ColdStorage { get; set; }
            public String Email { get; set; }
            public String Equifax { get; set; }
            public String Experian { get; set; }
            public String ExperianBulk { get; set; }
            public String FileStorage { get; set; }
            public String Graydon { get; set; }
            public String Hpi { get; set; }
            public String Iovation { get; set; }
            public String Salesforce { get; set; }
            public String Sms { get; set; }
            public String Timezone { get; set; }
            public String TransUnion { get; set; }
            public String Uru { get; set; }
            public String WongaPay { get; set; }

            public MsmqConfig(String server) : this(server, server) { }

            public MsmqConfig(String service, String component)
            {
                String format = @"FormatName:DIRECT=OS:{0}\private$\{1}";

                Ops = String.Format(format, service, "opsservice");
                Comms = String.Format(format, service, "commsservice");
                Payments = String.Format(format, service, "paymentsservice");
                Risk = String.Format(format, service, "riskservice");
                Marketing = String.Format(format, service, "marketingservice");
                Bi = String.Format(format, service, "biservice");

                BankGateway = String.Format(format, component, "bankgatewaytc");
                Blacklist = String.Format(format, component, "blacklistcomponent");
                BottomLine = String.Format(format, component, "bankgatewaybottomlinetc");
                CallReport = String.Format(format, component, "callreportcomponent");
                CallValidate = String.Format(format, component, "callvalidatecomponent");
                CardPayment = String.Format(format, component, "cardpaymentcomponent");
                ColdStorage = String.Format(format, component, "coldstoragecomponent");
                Email = String.Format(format, component, "emailcomponent");
                Equifax = String.Format(format, component, "equifaxcomponent");
                Experian = String.Format(format, component, "experiancomponent");
                ExperianBulk = String.Format(format, component, "experianbulkcomponent");
                FileStorage = String.Format(format, component, "filestoragecomponent");
                Graydon = String.Format(format, component, "graydoncomponent");
                Hpi = String.Format(format, component, "hpicomponent");
                Hsbc = String.Format(format, component, "bankgatewayhsbctc");
                Hyphen = String.Format(format, component, "bankgatewayhyphentc");
                Iovation = String.Format(format, component, "iovationcomponent");
                Salesforce = String.Format(format, component, "salesforcecomponent");
                Scotia = String.Format(format, component, "bankgatewayscotiatc");
                Sms = String.Format(format, component, "smscomponent");
                Timezone = String.Format(format, component, "timezonecomponent");
                TransUnion = String.Format(format, component, "transunioncomponent");
                Uru = String.Format(format, component, "urucomponent");
                WongaPay = String.Format(format, component, "wongapaytc");
            }
        }

        public class DbConfig
        {
            public String Ops { get; set; }
            public String OpsSagas { get; set; }
            public String OpsLogs { get; set; }
            public String Comms { get; set; }
            public String Payments { get; set; }
            public String Risk { get; set; }
            public String Bi { get; set; }
            public String BankGateway { get; set; }
            public String Blacklist { get; set; }
            public String CallReport { get; set; }
            public String CallValidate { get; set; }
            public String CardPayment { get; set; }
            public String ColdStorage { get; set; }
            public String ContactManagement { get; set; }
            public String Experian { get; set; }
            public String ExperianBulk { get; set; }
            public String FileStorage { get; set; }
            public String Hpi { get; set; }
            public String IpLookup { get; set; }
            public String Salesforce { get; set; }
            public String Scheduler { get; set; }
            public String Sms { get; set; }
            public String TimeZone { get; set; }
            public String TransUnion { get; set; }
            public String Uru { get; set; }
            public String WongaPay { get; set; }
            public String QAData { get; set; }

            public DbConfig(String server)
            {
                String format = @"Data Source={0};Initial Catalog={1};Integrated Security=True";

                Ops = String.Format(format, server, "Ops");
                OpsSagas = String.Format(format, server, "OpsSagas");
                OpsLogs = String.Format(format, server, "OpsLogs");
                Comms = String.Format(format, server, "Comms");
                Payments = String.Format(format, server, "Payments");
                Risk = String.Format(format, server, "Risk");
                Bi = String.Format(format, server, "Bi");
                BankGateway = String.Format(format, server, "BankGateway");
                Blacklist = String.Format(format, server, "Blacklist");
                CallReport = String.Format(format, server, "CallReport");
                CallValidate = String.Format(format, server, "CallValidate");
                CardPayment = String.Format(format, server, "CardPayment");
                ColdStorage = String.Format(format, server, "ColdStorage");
                ContactManagement = String.Format(format, server, "ContactManagement");
                Experian = String.Format(format, server, "Experian");
                ExperianBulk = String.Format(format, server, "ExperianBulk");
                FileStorage = String.Format(format, server, "FileStorage");
                Hpi = String.Format(format, server, "Hpi");
                IpLookup = String.Format(format, server, "IpLookup");
                Salesforce = String.Format(format, server, "Salesforce");
                Scheduler = String.Format(format, server, "Scheduler");
                Sms = String.Format(format, server, "Sms");
                TimeZone = String.Format(format, server, "TimeZone");
                TransUnion = String.Format(format, server, "TransUnion");
                Uru = String.Format(format, server, "Uru");
                WongaPay = String.Format(format, server, "WongaPay");
                QAData = String.Format(format, server, "QAData");
            }
        }

        public class UiConfig
        {
            public Uri Home { get; set; }

            public UiConfig(String host)
            {
                Home = new UriBuilder { Host = host }.Uri;
            }
        }
    }

    public static class Connections
    {
        private static Dictionary<string, string> DbPortMappings = new Dictionary<string, string>();

        static Connections()
        {
            DbPortMappings.Add("WIP2", "8201");
            DbPortMappings.Add("UAT2", "8202");
            DbPortMappings.Add("RC2", "8203");
            DbPortMappings.Add("WIP4", "8204");
            DbPortMappings.Add("UAT4", "8205");
            DbPortMappings.Add("RC4", "8206");
            DbPortMappings.Add("WIP6", "8207");
            DbPortMappings.Add("UAT6", "8208");
            DbPortMappings.Add("RC6", "8209");
        }

        public static string GetDbConn(string server, bool proxyMode)
        {
            if (proxyMode && DbPortMappings.ContainsKey(server))
                return string.Format("{0},{1}", server, DbPortMappings[server]);
            return server;
        }
    }
}