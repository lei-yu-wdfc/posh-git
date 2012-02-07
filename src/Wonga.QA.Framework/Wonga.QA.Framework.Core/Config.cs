using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;

namespace Wonga.QA.Framework.Core
{
    public enum AUT { Uk, Za, Ca, Wb }
    public enum SUT { Dev, WIP, UAT, RC }

    public static class Config
    {
        public static SUT SUT { get; set; }
        public static AUT AUT { get; set; }

        public static ApiConfig Api { get; set; }
        public static MsmqConfig Msmq { get; set; }
        public static DbConfig Db { get; set; }

        static Config()
        {
            SUT = GetValue<SUT>();
            AUT = GetValue<AUT>();

            switch (SUT)
            {
                case SUT.Dev:
                    Api = new ApiConfig("localhost");
                    Msmq = new MsmqConfig(".");
                    Db = new DbConfig(".");
                    break;
                case SUT.WIP:
                    Api = new ApiConfig(String.Format("wip.api.{0}.wonga.com", AUT));
                    Msmq =
                        AUT == AUT.Uk ? new MsmqConfig("WIP2") :
                        AUT == AUT.Za ? new MsmqConfig("WIP4") :
                        AUT == AUT.Ca ? new MsmqConfig("WIP6") :
                        AUT == AUT.Wb ? new MsmqConfig("WIP8") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Uk ? new DbConfig("WIP2") :
                        AUT == AUT.Za ? new DbConfig("WIP4") :
                        AUT == AUT.Ca ? new DbConfig("WIP6") :
                        AUT == AUT.Wb ? new DbConfig("WIP8") : Throw<DbConfig>();
                    break;
                case SUT.UAT:
                    Api = new ApiConfig(String.Format("uat.api.{0}.wonga.com", AUT));
                    Msmq =
                        AUT == AUT.Uk ? new MsmqConfig("UAT2") :
                        AUT == AUT.Za ? new MsmqConfig("UAT4") :
                        AUT == AUT.Ca ? new MsmqConfig("UAT6") :
                        AUT == AUT.Wb ? new MsmqConfig("UAT8") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Uk ? new DbConfig("UAT2") :
                        AUT == AUT.Za ? new DbConfig("UAT4") :
                        AUT == AUT.Ca ? new DbConfig("UAT6") :
                        AUT == AUT.Wb ? new DbConfig("UAT8") : Throw<DbConfig>();
                    break;
                case SUT.RC:
                    Api = new ApiConfig(String.Format("rc.api.{0}.wonga.com", AUT));
                    Msmq =
                        AUT == AUT.Uk ? new MsmqConfig("RC2") :
                        AUT == AUT.Za ? new MsmqConfig("RC4") :
                        AUT == AUT.Ca ? new MsmqConfig("RC6") :
                        AUT == AUT.Wb ? new MsmqConfig("RC9", "RC10") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Uk ? new DbConfig("RC2") :
                        AUT == AUT.Za ? new DbConfig("RC4") :
                        AUT == AUT.Ca ? new DbConfig("RC6") :
                        AUT == AUT.Wb ? new DbConfig("RC8") : Throw<DbConfig>();
                    break;
                default:
                    throw new NotImplementedException();
            }

            /*Trace.WriteLine(SUT, typeof(Config).FullName);
            Trace.WriteLine(AUT, typeof(Config).FullName);
            foreach (Object config in new Object[] { Api, Msmq, Db })
                foreach (PropertyInfo property in config.GetType().GetProperties())
                    Trace.WriteLine(String.Format("{0} = {1}", property.Name, property.GetValue(config, null)), config.GetType().FullName);*/
        }

        public static T Throw<T>()
        {
            throw new NotImplementedException();
        }

        private static T GetValue<T>()
        {
            return (T)Enum.Parse(typeof(T), (String)Registry.CurrentUser.OpenSubKey("Environment").GetValue(typeof(T).Name), true);
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

        public class MsmqConfig
        {
            public String Ops { get; set; }
            public String Comms { get; set; }
            public String Payments { get; set; }
            public String Risk { get; set; }
            public String Bi { get; set; }

            public String BankGateway { get; set; }
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

            public MsmqConfig(String server)
                : this(server, server)
            {
            }

            public MsmqConfig(String service, String component)
            {
                String format = @"FormatName:DIRECT=OS:{0}\private$\{1}";

                Ops = String.Format(format, service, "opsservice");
                Comms = String.Format(format, service, "commsservice");
                Payments = String.Format(format, service, "paymentsservice");
                Risk = String.Format(format, service, "riskservice");
                Bi = String.Format(format, service, "biservice");

                BankGateway = String.Format(format, component, "bankgatewaytc");
                Hyphen = String.Format(format, component, "bankgatewayhyphentc");
                Scotia = String.Format(format, component, "bankgatewayscotiatc");
                Blacklist = String.Format(format, component, "blacklistcomponent");
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
                Iovation = String.Format(format, component, "iovationcomponent");
                Salesforce = String.Format(format, component, "salesforcecomponent");
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
            }
        }
    }
}
