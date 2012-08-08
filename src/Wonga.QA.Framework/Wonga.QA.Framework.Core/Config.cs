using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Win32;

namespace Wonga.QA.Framework.Core
{
    public enum AUT { Uk, Za, Ca, Wb, Pl }
    public enum SUT { Dev, WIP, UAT, RC, WIPRelease, RCRelease, Live, WIPDI }

    public static class Config
    {
        public static SUT SUT { get; set; }
        public static AUT AUT { get; set; }
        public static Boolean Proxy { get; set; }

        public static ApiConfig Api { get; set; }
        public static CsConfig Cs { get; set; }
		public static UcgConfig Ucg { get; set; }
        public static SvcConfig Svc { get; set; }
        public static MsmqConfig Msmq { get; set; }
        public static DbConfig Db { get; set; }       
        public static UiConfig Ui { get; set; }
        public static AdminConfig Admin { get; set; }
        public static SalesforceConfig Salesforce { get; set; }
        public static EmailConfig Email { get; set; }
        public static PayLaterConfig PayLater { get; set; }
        public static PrepaidAdminConfig PrepaidAdminUI { get; set; }
        public static CommonApiConfig CommonApi { get; set; }
        private static XDocument _settings;

        static Config()
        {
            Configure();
        }

        public static void Configure(string executingDirectoryPath = null, string configsDirectoryPath = null, string appDataDirectoryPath = null, string testTarget = null)
        {
            var settingsManager = new SettingsManager(executingDirectoryPath, configsDirectoryPath, appDataDirectoryPath, testTarget);
            _settings = settingsManager.ReadSettings();
            if (_settings == null)
            {
                Trace.WriteLine(string.Format("WARNING: Configuration file for test target '{0}' was not found.\n" +
                                              "Configs Directory: {1}\n" +
                                              "Executing Directory: {2}", 
                                                settingsManager.TestTarget, 
                                                settingsManager.ConfigDirectoryPath, 
                                                settingsManager.ExecutingDirectoryPath));
                return;
            }

            Ui = new UiConfig();
            Db = new DbConfig();
            Salesforce = new SalesforceConfig();
            PayLater = new PayLaterConfig();
            Svc = new SvcConfig();
            Msmq = new MsmqConfig();

            SUT = Get.EnumFromString<SUT>(_settings.XPathSelectElement("//SUT").Value);
            AUT = Get.EnumFromString<AUT>(_settings.XPathSelectElement("//AUT").Value);

            Ui = new UiConfig();
            Ui.Browser = Get.EnumFromString<UiConfig.BrowserType>(GetSettingFromXml("//Ui/Browser"));
            Ui.BrowserVersion = GetSettingFromXml("//Ui/BrowserVersion");
            Ui.DoubleClickCookiesHome = GetSettingFromXml("//Ui/DoubleClickCookiesHome");
            Ui.Home = new Uri(GetSettingFromXml("//Ui/HomePage"));
            Ui.RemoteApiKey = GetSettingFromXml("//Ui/RemoteApiKey");
            Ui.RemoteMode = bool.Parse(GetSettingFromXml("//Ui/RemoteMode"));
            Ui.RemotePassword = GetSettingFromXml("//Ui/RemotePassword");
            Ui.RemoteUri = new Uri(GetSettingFromXml("//Ui/RemoteUri"));
            Ui.RemoteUsername = GetSettingFromXml("//Ui/RemoteUsername");

            Salesforce.Home = new Uri(GetSettingFromXml("//Salesforce/HomePage"));
            Salesforce.Password = GetSettingFromXml("//Salesforce/Password");
            Salesforce.Username = GetSettingFromXml("//Salesforce/Username");
            Salesforce.ApiPassword = GetSettingFromXml("//Salesforce/ApiPassword");
            Salesforce.ApiUrl = GetSettingFromXml("//Salesforce/ApiUrl");
            Salesforce.ApiUsername = GetSettingFromXml("//Salesforce/ApiUsername");

            PayLater.Home = new Uri(GetSettingFromXml("//PayLater/HomePage"));
            PayLater.Password = GetSettingFromXml("//PayLater/Password");
            PayLater.Username = GetSettingFromXml("//PayLater/Username");
            PayLater.ApiPassword = GetSettingFromXml("//PayLater/ApiPassword");
            PayLater.ApiUrl = GetSettingFromXml("//PayLater/ApiUrl");
            PayLater.ApiUsername = GetSettingFromXml("//PayLater/ApiUsername");

            Proxy = bool.Parse(GetSettingFromXml("//ProxyMode"));

            Email = new EmailConfig
            {
                QA = new EmailConfig.EmailAddressConfig
                {
                    Host = GetSettingFromXml("//Email/Host"),
                    Username = GetSettingFromXml("//Email/Username"),
                    Password = GetSettingFromXml("//Email/Password"),
                    Port = int.Parse(GetSettingFromXml("//Email/Port")),
                    IsSsl = bool.Parse(GetSettingFromXml("//Email/IsSsl"))
                }
            };
            Api = new ApiConfig(GetSettingFromXml("//Api/Url"));
            CommonApi = new CommonApiConfig(GetSettingFromXml("//CommonApi/Url"));
            Cs = new CsConfig(GetSettingFromXml("//CSApi/Url"));
			Ucg = new UcgConfig(GetSettingFromXml("//UCG/Url"));
            Admin = new AdminConfig { Home = new Uri(GetSettingFromXml("//Admin/HomePage")) };
            PrepaidAdminUI = new PrepaidAdminConfig
            {
                Home = new Uri(GetSettingFromXml("//PrepaidAdmin/HomePage")),
                User = GetSettingFromXml("//PrepaidAdmin/Username"),
                Pwd = GetSettingFromXml("//PrepaidAdmin/Password")
            };

            Svc.Ops = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Ops", "Name"), GetSettingFromXml("//Svc/Ops"));
            Svc.Comms = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Comms", "Name"), GetSettingFromXml("//Svc/Comms"));
            Svc.Payments = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Payments", "Name"), GetSettingFromXml("//Svc/Payments"));
            Svc.Risk = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Risk", "Name"), GetSettingFromXml("//Svc/Risk"));
            Svc.Marketing = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Marketing", "Name"), GetSettingFromXml("//Svc/Marketing"));
            Svc.Bi = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Bi", "Name"), GetSettingFromXml("//Svc/Bi"));

            Svc.BankGateway = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGateway", "Name"), GetSettingFromXml("//Svc/BankGateway"));
            Svc.BankGatewayBmo = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayBmo", "Name"), GetSettingFromXml("//Svc/BankGatewayBmo"));
            Svc.BankGatewayBottomLine = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayBottomLine", "Name"), GetSettingFromXml("//Svc/BankGatewayBottomLine"));
            Svc.BankGatewayHsbc = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayHsbc", "Name"), GetSettingFromXml("//Svc/BankGatewayHsbc"));
            Svc.BankGatewayHyphen = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayHyphen", "Name"), GetSettingFromXml("//Svc/BankGatewayHyphen"));
            Svc.BankGatewayRbc = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayRbc", "Name"), GetSettingFromXml("//Svc/BankGatewayRbc"));
            Svc.BankGatewayScotia = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayScotia", "Name"), GetSettingFromXml("//Svc/BankGatewayScotia"));
            Svc.BankGatewayEasyPay = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayEasyPay", "Name"), GetSettingFromXml("//Svc/BankGatewayEasyPay"));
            Svc.BankGatewayBre = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayBre", "Name"), GetSettingFromXml("//Svc/BankGatewayBre"));
            Svc.BankGatewayP24 = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BankGatewayP24", "Name"), GetSettingFromXml("//Svc/BankGatewayP24"));
            Svc.Blacklist = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/BlackList", "Name"), GetSettingFromXml("//Svc/BlackList"));
            Svc.CallReport = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/CallReport", "Name"), GetSettingFromXml("//Svc/CallReport"));
            Svc.CallValidate = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/CallValidate", "Name"), GetSettingFromXml("//Svc/CallValidate"));
            Svc.CardPayment = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/CardPayment", "Name"), GetSettingFromXml("//Svc/CardPayment"));
            Svc.ColdStorage = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/ColdStorage", "Name"), GetSettingFromXml("//Svc/ColdStorage"));
            Svc.ContactManagement = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/ContactManagement", "Name"), GetSettingFromXml("//Svc/ContactManagement"));
            Svc.DocumentGeneration = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/DocumentGeneration", "Name"), GetSettingFromXml("//Svc/DocumentGeneration"));
            Svc.Email = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Email", "Name"), GetSettingFromXml("//Svc/Email"));
            Svc.Equifax = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Equifax", "Name"), GetSettingFromXml("//Svc/Equifax"));
            Svc.Experian = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Experian", "Name"), GetSettingFromXml("//Svc/Experian"));
            Svc.ExperianBulk = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/ExperianBulk", "Name"), GetSettingFromXml("//Svc/ExperianBulk"));
            Svc.FileStorage = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/FileStorage", "Name"), GetSettingFromXml("//Svc/FileStorage"));
            Svc.Graydon = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Graydon", "Name"), GetSettingFromXml("//Svc/Graydon"));
            Svc.Hpi = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Hpi", "Name"), GetSettingFromXml("//Svc/Hpi"));
            Svc.Iovation = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Iovation", "Name"), GetSettingFromXml("//Svc/Iovation"));
            Svc.Salesforce = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Salesforce", "Name"), GetSettingFromXml("//Svc/Salesforce"));
            Svc.Scheduler = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Scheduler", "Name"), GetSettingFromXml("//Svc/Scheduler"));
            Svc.Sms = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Sms", "Name"), GetSettingFromXml("//Svc/Sms"));
            Svc.TimeoutManager = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/TimeoutManager", "Name"), GetSettingFromXml("//Svc/TimeoutManager"));
            Svc.TimeZone = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Timezone", "Name"), GetSettingFromXml("//Svc/Timezone"));
            Svc.TransUnion = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/TransUnion", "Name"), GetSettingFromXml("//Svc/TransUnion"));
            Svc.Uru = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/Uru", "Name"), GetSettingFromXml("//Svc/Uru"));
            Svc.WongaPay = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/WongaPay", "Name"), GetSettingFromXml("//Svc/WongaPay"));
            Svc.PayU = new KeyValuePair<String, String>(GetSettingAttributeFromXml("//Svc/PayU", "Name"), GetSettingFromXml("//Svc/PayU"));

            Msmq.Ops = GetSettingFromXml("//Msmq/Ops");
            Msmq.Comms = GetSettingFromXml("//Msmq/Comms");
            Msmq.Payments = GetSettingFromXml("//Msmq/Payments");
            Msmq.Risk = GetSettingFromXml("//Msmq/Risk");
            Msmq.Marketing = GetSettingFromXml("//Msmq/Marketing");
            Msmq.Bi = GetSettingFromXml("//Msmq/Bi");
            Msmq.BankGateway = GetSettingFromXml("//Msmq/BankGateway");
            Msmq.BankGatewayBmo = GetSettingFromXml("//Msmq/BankGatewayBmo");
            Msmq.BankGatewayRbc = GetSettingFromXml("//Msmq/BankGatewayRbc");
            Msmq.BankGatewayScotia = GetSettingFromXml("//Msmq/BankGatewayScotia");
            Msmq.BankGatewayBottomLine = GetSettingFromXml("//Msmq/BankGatewayBottomLine");
            Msmq.BankGatewayHyphen = GetSettingFromXml("//Msmq/BankGatewayHyphen");
            Msmq.BankGatewayEasyPay = GetSettingFromXml("//Msmq/BankGatewayEasyPay");
            Msmq.BankGatewayHsbc = GetSettingFromXml("//Msmq/BankGatewayHsbc");
            Msmq.Blacklist = GetSettingFromXml("//Msmq/BlackList");
            Msmq.CallReport = GetSettingFromXml("//Msmq/CallReport");
            Msmq.CallValidate = GetSettingFromXml("//Msmq/CallValidate");
            Msmq.CardPayment = GetSettingFromXml("//Msmq/CardPayment");
            Msmq.ColdStorage = GetSettingFromXml("//Msmq/ColdStorage");
            Msmq.Email = GetSettingFromXml("//Msmq/Email");
            Msmq.Equifax = GetSettingFromXml("//Msmq/Equifax");
            Msmq.Experian = GetSettingFromXml("//Msmq/Experian");
            Msmq.ExperianBulk = GetSettingFromXml("//Msmq/ExperianBulk");
            Msmq.FileStorage = GetSettingFromXml("//Msmq/FileStorage");
            Msmq.Graydon = GetSettingFromXml("//Msmq/Graydon");
            Msmq.Hpi = GetSettingFromXml("//Msmq/Hpi");
            Msmq.Iovation = GetSettingFromXml("//Msmq/Iovation");
            Msmq.Salesforce = GetSettingFromXml("//Msmq/Salesforce");
            Msmq.Sms = GetSettingFromXml("//Msmq/Sms");
            Msmq.SmsDistributor = GetSettingFromXml("//Msmq/SmsDistributor");
            Msmq.Timezone = GetSettingFromXml("//Msmq/Timezone");
            Msmq.TransUnion = GetSettingFromXml("//Msmq/TransUnion");
            Msmq.Uru = GetSettingFromXml("//Msmq/Uru");
            Msmq.WongaPay = GetSettingFromXml("//Msmq/WongaPay");
            Msmq.PayU = GetSettingFromXml("//Msmq/PayU");

            Db.Accounting = GetSettingFromXml("//Db/Accounting");
            Db.BankGateway = GetSettingFromXml("//Db/BankGateway");
            Db.Bi = GetSettingFromXml("//Db/Bi");
            Db.BiCustomerManagement = GetSettingFromXml("//Db/BiCustomerManagement");
            Db.Blacklist = GetSettingFromXml("//Db/BlackList");
            Db.CallReport = GetSettingFromXml("//Db/CallReport");
            Db.CallValidate = GetSettingFromXml("//Db/CallValidate");
            Db.CardPayment = GetSettingFromXml("//Db/CardPayment");
            Db.Cdc = GetSettingFromXml("//Db/Cdc");
            Db.ColdStorage = GetSettingFromXml("//Db/ColdStorage");
            Db.Comms = GetSettingFromXml("//Db/Comms");
            Db.ContactManagement = GetSettingFromXml("//Db/ContactManagement");
            Db.DiControl = GetSettingFromXml("//Db/DiControl");
            Db.DiStaging = GetSettingFromXml("//Db/DiStaging");
            Db.DiStagingWonga = GetSettingFromXml("//Db/DiStagingWonga");
            Db.Experian = GetSettingFromXml("//Db/Experian");
            Db.ExperianBulk = GetSettingFromXml("//Db/ExperianBulk");
            Db.FileStorage = GetSettingFromXml("//Db/FileStorage");
            Db.GreyfaceShell = GetSettingFromXml("//Db/GreyfaceShell");
            Db.Hds = GetSettingFromXml("//Db/Hds");
            Db.Hpi = GetSettingFromXml("//Db/Hpi");
            Db.IpLookup = GetSettingFromXml("//Db/IpLookup");
            Db.Marketing = GetSettingFromXml("//Db/Marketing");
            Db.Ops = GetSettingFromXml("//Db/Ops");
            Db.OpsLogs = GetSettingFromXml("//Db/OpsLogs");
            Db.OpsSagas = GetSettingFromXml("//Db/OpsSagas");
            Db.Payments = GetSettingFromXml("//Db/Payments");
            Db.Pps = GetSettingFromXml("//Db/Pps");
            Db.PrepaidCard = GetSettingFromXml("//Db/PrepaidCard");
            Db.QaData = GetSettingFromXml("//Db/QaData");
            Db.Risk = GetSettingFromXml("//Db/Risk");
            Db.Salesforce = GetSettingFromXml("//Db/Salesforce");
            Db.Scheduler = GetSettingFromXml("//Db/Scheduler");
            Db.Sms = GetSettingFromXml("//Db/Sms");
            Db.TimeZone = GetSettingFromXml("//Db/Timezone");
            Db.TransUnion = GetSettingFromXml("//Db/TransUnion");
            Db.Uru = GetSettingFromXml("//Db/Uru");
            Db.Warehouse = GetSettingFromXml("//Db/Warehouse");
            Db.WongaPay = GetSettingFromXml("//Db/WongaPay");
            Db.WongaWholeStaging = GetSettingFromXml("//Db/WongaWholeStaging");
            Db.MigrationStaging = GetSettingFromXml("//Db/MigrationStaging");

            Trace.WriteLine(SUT, typeof(Config).FullName);
            Trace.WriteLine(AUT, typeof(Config).FullName);
        }

        private static string GetSettingFromXml(string xpath)
        {
            var element = _settings.XPathSelectElement(xpath);
            var encrypted = element.Attribute("E") != null ? element.Attribute("E").Value : "false";
            if (encrypted == "true")
                return Class.Decrypt(element.Value);
            return element.Value;
        }

        private static string GetSettingAttributeFromXml(string xpath, string attr)
        {
            return _settings.XPathSelectElement(xpath).Attribute(attr).Value;
        }

        public static T Throw<T>()
        {
            throw new NotImplementedException(typeof(T).FullName);
        }

        public static T ThrowInvalidConfiguration<T>(string reason)
        {
            throw new ConfigurationErrorsException(reason);
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

		public class UcgConfig
		{
			public Uri Commands { get; set; }
			public Uri Queries { get; set; }

			public UcgConfig(String host)
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

            public KeyValuePair<String, String> BankGateway { get; set; }
            public KeyValuePair<String, String> BankGatewayBmo { get; set; }
            public KeyValuePair<String, String> BankGatewayBottomLine { get; set; }
            public KeyValuePair<String, String> BankGatewayHsbc { get; set; }
            public KeyValuePair<String, String> BankGatewayHyphen { get; set; }
            public KeyValuePair<String, String> BankGatewayRbc { get; set; }
            public KeyValuePair<String, String> BankGatewayScotia { get; set; }
            public KeyValuePair<String, String> BankGatewayEasyPay { get; set; }
            public KeyValuePair<String, String> BankGatewayBre { get; set; }
            public KeyValuePair<String, String> BankGatewayP24 { get; set; }
            public KeyValuePair<String, String> Blacklist { get; set; }
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
            public KeyValuePair<String, String> Iovation { get; set; }
            public KeyValuePair<String, String> Salesforce { get; set; }
            public KeyValuePair<String, String> Scheduler { get; set; }
            public KeyValuePair<String, String> Sms { get; set; }
            public KeyValuePair<String, String> TimeoutManager { get; set; }
            public KeyValuePair<String, String> TimeZone { get; set; }
            public KeyValuePair<String, String> TransUnion { get; set; }
            public KeyValuePair<String, String> Uru { get; set; }
            public KeyValuePair<String, String> WongaPay { get; set; }
			public KeyValuePair<String, String> PayU { get; set; }
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
			public String BankGatewayRbc { get; set; }
            public String BankGatewayBottomLine { get; set; }
            public String BankGatewayHsbc { get; set; }
            public String BankGatewayHyphen { get; set; }
            public String BankGatewayEasyPay { get; set; }
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
			public String PayU { get; set; }
        }

        public class DbConfig
        {
            public String Accounting { get; set; }
            public String Ops { get; set; }
            public String Comms { get; set; }
            public String Payments { get; set; }
            public String Risk { get; set; }
            public String Bi { get; set; }
            public String BiCustomerManagement { get; set; }

            public String BankGateway { get; set; }
            public String Hds { get; set; }
            public String Blacklist { get; set; }
            public String CallReport { get; set; }
            public String CallValidate { get; set; }
            public String CardPayment { get; set; }
            public String Cdc { get; set; }
            public String ColdStorage { get; set; }
            public String ContactManagement { get; set; }
            public String DiControl { get; set; }
            public String DiStaging { get; set; }
            public String DiStagingWonga { get; set; }
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
            public String Warehouse { get; set; }
            public String WongaPay { get; set; }
            public String Marketing { get; set; }
            public String PrepaidCard { get; set; }
            public String Pps { get; set; }
            public String WongaWholeStaging { get; set; }
            public String GreyfaceShell { get; set; }
            public String OpsLogs { get; set; }
            public String OpsSagas { get; set; }
            public String QaData { get; set; }
            public String MigrationStaging { get; set; }
        }

        public class UiConfig
        {
            public enum BrowserType
            {
                InternetExplorer,
                Firefox,
                Safari,
                Chrome,
                Opera,
                FirefoxMobile,
                Android
            }

            public string Url { get { return Home.AbsoluteUri; } }
            public Uri Home { get; set; }
            public string DoubleClickCookiesHome { get; set; }

            /// <summary>
            /// Username for the selenium rc server
            /// </summary>
            public string RemoteUsername { get; set; }
            /// <summary>
            /// Password for the selenium rc server
            /// </summary>
            public string RemotePassword { get; set; }
            /// <summary>
            /// API key for the Selenium RC server
            /// </summary>
            public string RemoteApiKey { get; set; }
            /// <summary>
            /// Selenium RC Url
            /// </summary>
            public Uri RemoteUri { get; set; }

            public BrowserType Browser { get; set; }
            public string BrowserVersion { get; set; }

            internal UiConfig()
            {
                
            }

            internal void SetUri(string host)
            {
                Home = new UriBuilder { Host = host }.Uri;
            }
            internal void SetUri(string host, int portNumber)
            {
                Home = new UriBuilder { Scheme = string.Empty, Host = host, Port = portNumber }.Uri;
            }

            public bool RemoteMode { get; set; }
        }

        public class AdminConfig
        {
            public Uri Home { get; set; }
        }

        public class PrepaidAdminConfig
        {
            public Uri Home { get; set; }
            public String User { get; set; }
            public String Pwd { get; set; }
        }

        public class SalesforceConfig
        {
            public Uri Home { get; set; }
            public String Username { get; set; }
            public String Password { get; set; }
            public String ApiUrl { get; set; }
            public String ApiUsername { get; set; }
            public String ApiPassword { get; set; }
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
            public String Username { get; set; }
            public String Password { get; set; }
            public String ApiUrl { get; set; }
            public String ApiUsername { get; set; }
            public String ApiPassword { get; set; }
        }

        public class CommonApiConfig
        {
            public Uri Commands { get; set; }
            public CommonApiConfig(String host)
            {
                Uri uri = new UriBuilder { Host = host }.Uri;
                Commands = new Uri(uri, "commands");
            }
        }
    }
}
