using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Win32;

namespace Wonga.QA.Framework.Core
{
    public enum AUT { Uk, Za, Ca, Wb, Pl }
    public enum SUT { Dev, WIP, UAT, RC, WIPRelease, RCRelease }

    public static class Config
    {
        public static SUT SUT { get; set; }
        public static AUT AUT { get; set; }
        public static Boolean Proxy { get; set; }

        public static ApiConfig Api { get; set; }
        public static CsConfig Cs { get; set; }
        public static SvcConfig Svc { get; set; }
        public static MsmqConfig Msmq { get; set; }
        public static DbConfig Db { get; set; }
        public static UiConfig Ui { get; set; }
        public static AdminConfig Admin { get; set; }
        public static SalesforceConfig SalesforceUi { get; set; }
        public static SalesforceConfig SalesforceApi { get; set; }
        public static EmailConfig Email { get; set; }
        public static PayLaterConfig PayLaterUi { get; set; }
        public static PayLaterConfig PayLaterApi { get; set; }
        public static CommonApiConfig CommonApi { get; set; }


        static Config()
        {
            SUT = GetValue<SUT>();
            AUT = GetValue<AUT>();

            Ui = new UiConfig();
            SalesforceUi = new SalesforceConfig("test.salesforce.com");
            PayLaterUi = new PayLaterConfig("dev.paylater.com");

            Proxy = GetValue<Boolean>(false, "QAFProxyMode");

            Ui.Browser = GetValue<UiConfig.BrowserType>("FireFox", "QAFBrowser");
            Ui.BrowserVersion = GetValue<string>("", "QAFBrowserVersion");
            Ui.RemoteMode = GetValue<Boolean>(false, "QAFUiRemoteMode");
            Ui.ExternalAccess = GetValue<Boolean>(false, "QAFExternalAccessMode");

            Email = new EmailConfig() { QA = new EmailConfig.EmailAddressConfig() { Host = "imap.gmail.com", Username = "qa.wonga.com@gmail.com", Password = "Allw0nga", Port = 993, IsSsl = true } };
            switch (SUT)
            {
                case SUT.Dev:
                    Api = new ApiConfig("localhost");
                    CommonApi = new CommonApiConfig("localhost/IVRWebApi");
                    Cs = new CsConfig("localhost/CSAPI");
                    Svc = new SvcConfig(".");
                    Msmq = new MsmqConfig(".");
                    Db = new DbConfig(".");
                    Ui.SetUri("localhost");
                    Admin = new AdminConfig("localhost/admin");					
                    SalesforceUi.SetLoginDetails("qa.wonga.com@gmail.com.wip", "Allw0nga");
                    SalesforceApi =
                        AUT == AUT.Ca ? new SalesforceApiConfig("v3integration@wonga.com.int") :
                        new SalesforceApiConfig("v3integration@wonga.com.wip");
                    break;
                case SUT.WIP:
                    Api = new ApiConfig(String.Format("wip.api.{0}.wonga.com", AUT));
                    Cs = new CsConfig(String.Format("wip.csapi.{0}.wonga.com", AUT));
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
                        AUT == AUT.Uk ? new DbConfig(Connections.GetDbConn("WIP2", Proxy)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("WIP4", Proxy)) :
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("WIP6", Proxy)) :
                        AUT == AUT.Wb ? new DbConfig(Connections.GetDbConn("WIP8", Proxy)) : Throw<DbConfig>();
                    Ui.SetUri(String.Format("wip.{0}.wonga.com", AUT));
                    Admin = new AdminConfig(String.Format("wip.admin.{0}.wonga.com", AUT));
                    SalesforceUi.SetLoginDetails("qa.wonga.com@gmail.com.wip", "Allw0nga");
                    SalesforceApi =
                        AUT == AUT.Ca ? new SalesforceApiConfig("v3integration@wonga.com.int") :
                        new SalesforceApiConfig("v3integration@wonga.com.wip");
                    break;
                case SUT.WIPRelease:
                    Api = new ApiConfig(String.Format("wip.release.api.{0}.wonga.com", AUT));
                    Cs = new CsConfig(String.Format("wip.release.csapi.{0}.wonga.com", AUT));
                    Svc =
                        AUT == AUT.Ca ? new SvcConfig("ca-rel-wip-app") :
                        AUT == AUT.Za ? new SvcConfig("za-rel-wip-app") : Throw<SvcConfig>();
                    Msmq =
                        AUT == AUT.Ca ? new MsmqConfig("ca-rel-wip-app") :
                        AUT == AUT.Za ? new MsmqConfig("za-rel-wip-app") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("ca-rel-wip-app", Proxy)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("za-rel-wip-app", Proxy)) : Throw<DbConfig>();
                    Ui.SetUri(String.Format("wip.release.{0}.wonga.com", AUT));
                    Admin = new AdminConfig(String.Format("wip.release.admin.{0}.wonga.com", AUT));
                    SalesforceUi.SetLoginDetails("qa.wonga.com@gmail.com.wip", "Allw0nga");
                    SalesforceApi =
                        AUT == AUT.Ca ? new SalesforceApiConfig("v3integration@wonga.com.int") :
                        new SalesforceApiConfig("v3integration@wonga.com.wip");
                    break;
                case SUT.UAT:
                    Api = new ApiConfig(String.Format("uat.api.{0}.wonga.com", AUT));
                    Cs = new CsConfig(String.Format("uat.csapi.{0}.wonga.com", AUT));
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
                        AUT == AUT.Uk ? new DbConfig(Connections.GetDbConn("UAT2", Proxy)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("UAT4", Proxy)) :
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("UAT6", Proxy)) :
                        AUT == AUT.Wb ? new DbConfig(Connections.GetDbConn("UAT8", Proxy)) : Throw<DbConfig>();
                    Ui.SetUri(String.Format("uat.{0}.wonga.com", AUT));
                    Admin = new AdminConfig(String.Format("uat.admin.{0}.wonga.com", AUT));
                    break;
                case SUT.RC:
                    Api = new ApiConfig(String.Format("rc.api.{0}.wonga.com", AUT));
                    Cs = new CsConfig(String.Format("rc.csapi.{0}.wonga.com", AUT));
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
                        AUT == AUT.Uk ? new DbConfig(Connections.GetDbConn("RC2", Proxy)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("RC4", Proxy)) :
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("RC6", Proxy)) :
                        AUT == AUT.Wb ? new DbConfig(Connections.GetDbConn("RC8", Proxy)) : Throw<DbConfig>();
                    Ui.SetUri(String.Format("rc.{0}.wonga.com", AUT));
                    Admin = new AdminConfig(String.Format("rc.admin.{0}.wonga.com", AUT));
                    SalesforceUi.SetLoginDetails("qa.wonga.com@gmail.com.rc", "Allw0nga");
                    SalesforceApi = new SalesforceApiConfig("v3integration@wonga.com.rc");
                    break;
                case SUT.RCRelease:
                    Api = new ApiConfig(String.Format("rc.release.api.{0}.wonga.com", AUT));
                    Cs = new CsConfig(String.Format("rc.release.csapi.{0}.wonga.com", AUT));
                    Svc =
                        AUT == AUT.Ca ? new SvcConfig("ca-rel-rc-app") :
                        AUT == AUT.Za ? new SvcConfig("za-rel-rc-app") : Throw<SvcConfig>();
                    Msmq =
                        AUT == AUT.Ca ? new MsmqConfig("ca-rel-rc-app") :
                        AUT == AUT.Za ? new MsmqConfig("za-rel-rc-app") : Throw<MsmqConfig>();
                    Db =
                        AUT == AUT.Ca ? new DbConfig(Connections.GetDbConn("ca-rel-rc-app", Proxy)) :
                        AUT == AUT.Za ? new DbConfig(Connections.GetDbConn("za-rel-rc-app", Proxy)) : Throw<DbConfig>();
                    Ui.SetUri(String.Format("rc.release.{0}.wonga.com", AUT));
                    Admin = new AdminConfig(String.Format("rc.release.admin.{0}.wonga.com", AUT));
                    SalesforceUi.SetLoginDetails("qa.wonga.com@gmail.com.rc", "Allw0nga");
                    SalesforceApi = new SalesforceApiConfig("v3integration@wonga.com.rc");
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

        private static T GetValue<T>(object defaultValue = null, string variable = null)
        {
            Object value = Registry.CurrentUser.OpenSubKey("Environment").GetValue(variable ?? typeof(T).Name) ??
                defaultValue ??
                default(T);

            return (T)(typeof(T).IsEnum ? Enum.Parse(typeof(T), value.ToString(), true) : Convert.ChangeType(value, typeof(T)));
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

        public class CsConfig : ApiConfig
        {
            public CsConfig(String host) : base(host) { }
        }

        public class SvcConfig
        {
            public KeyValuePair<String, String> Ops { get; set; }
            public KeyValuePair<String, String> Comms { get; set; }
            public KeyValuePair<String, String> Payments { get; set; }
            public KeyValuePair<String, String> Risk { get; set; }
            public KeyValuePair<String, String> Marketing { get; set; }
            public KeyValuePair<String, String> Bi { get; set; }

            public KeyValuePair<String, String> Accounting { get; set; }
            public KeyValuePair<String, String> BankGateway { get; set; }
            public KeyValuePair<String, String> Blacklist { get; set; }
            public KeyValuePair<String, String> Bmo { get; set; }
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
                Bmo = new KeyValuePair<String, String>("Wonga.BankGateway.Bmo.Handlers", component);
                BottomLine = new KeyValuePair<String, String>("Wonga.BankGateway.Bottomline.Handlers", component);
                CallReport = new KeyValuePair<String, String>("Wonga.CallReport.Handlers", component);
                CallValidate = new KeyValuePair<String, String>("Wonga.CallValidate.Handlers", component);
                CardPayment = new KeyValuePair<String, String>("Wonga.CardPayment.Handlers", component);
                ColdStorage = new KeyValuePair<String, String>("Wonga.Payments.ColdStorage.Handlers", component);
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
            public String BankGatewayBmo { get; set; }
            public String BankGatewayScotia { get; set; }
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
            public String SmsDistributor { get; set; }
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
                BankGatewayBmo = String.Format(format, component, "bankgatewaybmotc");
                BankGatewayScotia = String.Format(format, component, "bankgatewayscotiatc");
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
                SmsDistributor = String.Format(format, component, "smsdistributorcomponent");
                Timezone = String.Format(format, component, "timezonecomponent");
                TransUnion = String.Format(format, component, "transunioncomponent");
                Uru = String.Format(format, component, "urucomponent");
                WongaPay = String.Format(format, component, "wongapaytc");
            }
        }

        public class DbConfig
        {
            public String Accounting { get; set; }
            public String Ops { get; set; }
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
            public String Marketing { get; set; }

            public String OpsLogs { get; set; }
            public String OpsSagas { get; set; }
            public String QaData { get; set; }

            public DbConfig(String server)
            {
                Func<String, String> builder = catalog => new SqlConnectionStringBuilder { DataSource = server, InitialCatalog = catalog, IntegratedSecurity = true }.ConnectionString;

                Accounting = builder("Accounting");
                Ops = builder("Ops");
                OpsLogs = builder("OpsLogs");
                OpsSagas = builder("OpsSagas");
                Comms = builder("Comms");
                Payments = builder("Payments");
                Risk = builder("Risk");
                Bi = builder("Bi");
                BankGateway = builder("BankGateway");
                Blacklist = builder("Blacklist");
                CallReport = builder("CallReport");
                CallValidate = builder("CallValidate");
                CardPayment = builder("CardPayment");
                ColdStorage = builder("ColdStorage");
                ContactManagement = builder("ContactManagement");
                Experian = builder("Experian");
                ExperianBulk = builder("ExperianBulk");
                FileStorage = builder("FileStorage");
                Hpi = builder("Hpi");
                IpLookup = builder("IpLookup");
                QaData = builder("QaData");
                Salesforce = builder("Salesforce");
                Scheduler = builder("Scheduler");
                Sms = builder("Sms");
                TimeZone = builder("TimeZone");
                TransUnion = builder("TransUnion");
                Uru = builder("Uru");
                WongaPay = builder("WongaPay");
                Marketing = builder("Marketing");
            }
        }

        public class UiConfig
        {
            public enum BrowserType
            {
                InternetExplorer,
                Firefox,
                Safari,
                Chrome,
                Opera
            }

            public string Url { get { return Home.AbsoluteUri; } }
            public Uri Home { get; set; }

            /// <summary>
            /// Username for the selenium rc server
            /// </summary>
            public string RemoteUsername { get { return "WongaQA"; } }
            /// <summary>
            /// Password for the selenium rc server
            /// </summary>
            public string RemotePassword { get { return "Passw0rd"; } }
            /// <summary>
            /// API key for the Selenium RC server
            /// </summary>
            public string RemoteApiKey { get { return "cb7d5fc7-44cd-42e6-a02b-a23d79671a3a"; } }
            /// <summary>
            /// Selenium RC Url
            /// </summary>
            public Uri RemoteUri { get { return new Uri("http://ondemand.saucelabs.com:80/wd/hub"); } }

            /// <summary>
            /// Whether or not we are accessing QA webpages via an external partner ie SauceLabs.
            /// </summary>
            public bool ExternalAccess { get; set; }
            /// <summary>
            /// Local to External address for our UI QA environment
            /// </summary>
            public Dictionary<string, string> ExternalAccessMap { get; private set; }

            public BrowserType Browser { get; set; }
            public string BrowserVersion { get; set; }

            internal UiConfig()
            {
                ExternalAccessMap = new Dictionary<string, string>();
                ExternalAccessMap.Add("rc.Uk.wonga.com", "");
                ExternalAccessMap.Add("rc.Wb.wonga.com", "");
                ExternalAccessMap.Add("rc.Za.wonga.com", "");
                ExternalAccessMap.Add("rc.Ca.wonga.com", "");
                ExternalAccessMap.Add("rc.release.Uk.wonga.com", "");
                ExternalAccessMap.Add("rc.release.Wb.wonga.com", "");
                ExternalAccessMap.Add("rc.release.Za.wonga.com", "");
                ExternalAccessMap.Add("rc.release.Ca.wonga.com", "");
                ExternalAccessMap.Add("wip.Uk.wonga.com", "");
                ExternalAccessMap.Add("wip.Wb.wonga.com", "");
                ExternalAccessMap.Add("wip.Za.wonga.com", "");
                ExternalAccessMap.Add("wip.Ca.wonga.com", "");
                ExternalAccessMap.Add("wip.release.Uk.wonga.com", "");
                ExternalAccessMap.Add("wip.release.Wb.wonga.com", "");
                ExternalAccessMap.Add("wip.release.Za.wonga.com", "");
                ExternalAccessMap.Add("wip.release.Ca.wong.com", "");
            }

            internal void SetUri(string host)
            {
                host = ExternalAccess ? ExternalAccessMap[host] : host;
                Home = new UriBuilder { Host = host }.Uri;
            }

            public bool RemoteMode { get; set; }
        }

        public class AdminConfig
        {

            public Uri Home { get; set; }

            public AdminConfig(String host)
            {
                Home = new UriBuilder {Host = host}.Uri;
            }
        }

        public class SalesforceConfig
        {
            public Uri Home { get; set; }
            public String Username { get; private set; }
            public String Password { get; private set; }

            public SalesforceConfig(String host)
            {
                Home = new UriBuilder { Host = host }.Uri;
            }

            public SalesforceConfig(Uri uri, string usernname, string password)
            {
                Home = uri;
                Username = usernname;
                Password = password;
            }

            public void SetLoginDetails(string username, string password)
            {
                Username = username;
                Password = password;
            }
        }

        public class SalesforceApiConfig : SalesforceConfig
        {
            public SalesforceApiConfig(string username)
                : base(new Uri("https://test.salesforce.com/services/Soap/c/23.0/0DFD0000000Drwo"), username, "7h2oieg0482h5gqh6R8sbJFQiLuFJUwe61yhB2yTq")
            {

            }
        }

        public class EmailConfig
        {
            public struct EmailAddressConfig
            {
                public string Host { get; set; }
                public string Username { get; set; }
                public string Password { get; set; }
                public bool IsSsl { get; set; }
                public int Port { get; set; }
            }

            public EmailAddressConfig QA { get; set; }
        }

        public class PayLaterConfig
        {
            public Uri Home { get; set; }
            public String Username { get; private set; }
            public String Password { get; private set; }

            public PayLaterConfig(String host)
            {
                Home = new UriBuilder { Host = host }.Uri;
            }

            public PayLaterConfig(Uri uri, string usernname, string password)
            {
                Home = uri;
                Username = usernname;
                Password = password;
            }

            public void SetLoginDetails(string username, string password)
            {
                Username = username;
                Password = password;
            }
        }

        public class PayLaterApiConfig : PayLaterConfig
        {
            public PayLaterApiConfig(string username)
                : base(new Uri("http://dev.paylater.com/"), username, "Passw0rd")
            {

            }
        }

        public class CommonApiConfig        {            public Uri Commands { get; set; }            public CommonApiConfig(String host)            {                Uri uri = new UriBuilder { Host = host }.Uri;                Commands = new Uri(uri, "commands");            }        }    }

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
