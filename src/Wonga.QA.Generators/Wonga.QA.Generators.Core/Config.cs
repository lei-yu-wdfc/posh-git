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
        public static Regex Artifact { get; set; }
        public static Regex Cs { get; set; }

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
            Origin = @"..\v3";
            SqlMetal = new FileInfo(Path.Combine(Repo.Lib.FullName, "SqlMetal\\SqlMetal.exe"));
            Test = new Regex(@"\.Tests?(\.|$)", RegexOptions.IgnoreCase);
            Artifact = new Regex(@"bin|obj");
            Cs = new Regex(@"\.Csapi\.", RegexOptions.IgnoreCase);

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
                "WongaPay",
                "QAData"
            };

            Solutions = new Dictionary<String, String>
            {
                { "Ops", "Ops" },
                { "Comms", "Comms" },
                { "Payments", "Payments" },
                { "Risk", "Risk" },
                { "Marketing", "Marketing" },
                { "Bi", "Bi" },
                { "Address", "Address" },
                { "BankGateway", "BankGateway" },
                { "BankValidate", "BankValidate" },
                { "BlackList", "Blacklist" },
                { "CallReport", "CallReport" },
                { "CallValidate", "CallValidate" },
                { "CardPayment", "CardPayment" },
                { "ColdStorage", "ColdStorage" },
                { "Comms.ContactManagement", "ContactManagement" },
                { "Comms.DocumentGeneration", "DocumentGeneration" },
                { "Email", "Email" },
                { "Equifax", "Equifax" },
                { "Experian", "Experian" },
                { "ExperianBulk", "ExperianBulk" },
                { "FileStorage", "FileStorage" },
                { "Graydon", "Graydon" },
                { "HPI", "Hpi" },
                { "Iovation", "Iovation" },
                { "IPLookup", "IpLookup" },
                { "Salesforce", "Salesforce" },
                { "Scheduler", "Scheduler" },
                { "Sms", "Sms" },
                { "TimeZone", "TimeZone" },
                { "Transunion", "TransUnion" },
                { "URU", "Uru" },
                { "WongaPay", "WongaPay" },

                //Anomalies
                { "Api", "Common" },
                {"PublicMessages.Bi", "Bi"},
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
