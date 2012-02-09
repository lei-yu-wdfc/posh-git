using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Wonga.QA.Generators.Core
{
    public static class Config
    {
        public static String Origin { get; set; }
        public static FileInfo SqlMetal { get; set; }
        public static Regex Test { get; set; }

        public static String[] Databases { get; set; }
        public static String[] Roots { get; set; }
        public static Dictionary<String, String> Solutions { get; set; }

        public static String Message { get; set; }
        public static String Request { get; set; }
        public static String Command { get; set; }
        public static String Query { get; set; }

        public static String[] Products { get; set; }
        public static String[] Regions { get; set; }
        public static String[] Suffixes { get; set; }

        public static Framework Api { get; set; }
        public static Framework Msmq { get; set; }
        public static Framework Db { get; set; }
        public static Framework Enums { get; set; }

        static Config()
        {
            Origin = @"..\..\..\v3";
            SqlMetal = new FileInfo(@"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\NETFX 4.0 Tools\x64\SqlMetal.exe");
            Test = new Regex(@"\.Tests?(\.|$)");

            Message = "NServiceBus.IMessage";
            Request = "Wonga.Api.IApiRequest";
            Command = "Wonga.Api.Command";
            Query = "Wonga.Api.IQuery";

            Roots = new[] { "src", "components" };
            Products = new[] { "Wb" };
            Regions = new[] { "Uk", "Za", "Ca" };
            Suffixes = new[] { "Command", "Query", "Event", "Message" };

            Api = new Framework { Project = String.Format("{0}.Api", Framework.Solution), Base = "ApiRequest", Folder = "Requests" };
            Msmq = new Framework { Project = String.Format("{0}.Msmq", Framework.Solution), Base = "MsmqMessage", Folder = "Messages" };
            Db = new Framework { Project = String.Format("{0}.Db", Framework.Solution), Base = "DbEntity", Folder = "Databases" };
            Enums = new Framework { Project = String.Format("{0}.Core", Framework.Solution), Folder = "Enums" };

            Databases = new[]
            {
                "Ops",
                "OpsLogs",
                "Comms",
                "Payments",
                "Risk",
                "Bi",
                //"Marketing",
                "BankGateway",
                "Blacklist",
                "CallReport",
                "CallValidate",
                "CardPayment",
                "ColdStorage",
                "ContactManagement",
                "Experian",
                "ExperianBulk",
                "FileStorage",
                "Hpi",
                "IpLookup",
                "Salesforce",
                "Scheduler",
                "Sms",
                "TimeZone",
                "TransUnion",
                "Uru",
                "WongaPay"
            };

            Solutions = new Dictionary<String, String>
            {
                {"common", "Common"},
                {"integration", "Integration"},
                {"ops", "Ops"},
                {"comms", "Comms"},
                {"marketing", "Marketing"},
                {"payments", "Payments"},
                {"risk", "Risk"},
                {"bi", "Bi"},
                {"Address", "Address"},
                {"BankGateway", "BankGateway"},
                {"BankValidate", "BankValidate"},
                {"BlackList", "Blacklist"},
                {"CallReport", "CallReport"},
                {"CallValidate", "CallValidate"},
                {"CardPayment", "CardPayment"},
                {"ColdStorage", "ColdStorage"},
                {"Comms.ContactManagement", "ContactManagement"},
                {"Comms.DocumentGeneration", "DocumentGeneration"},
                {"Email", "Email"},
                {"Equifax", "Equifax"},
                {"Experian", "Experian"},
                {"ExperianBulk", "ExperianBulk"},
                {"FileStorage", "FileStorage"},
                {"Graydon", "Graydon"},
                {"HPI", "Hpi"},
                {"Iovation", "Iovation"},
                {"IPLookup", "IpLookup"},
                {"Salesforce", "Salesforce"},
                {"Scheduler", "Scheduler"},
                {"Sms", "Sms"},
                {"TimeZone", "TimeZone"},
                {"Transunion", "TransUnion"},
                {"URU", "Uru"},
                {"WongaPay", "WongaPay"}
            };
        }

        public class Framework
        {
            public static String Solution { get { return "Wonga.QA.Framework"; } }
            public String Project { get; set; }
            public String Base { get; set; }
            public String Folder { get; set; }
        }
    }
}
