using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Wonga.QA.Generators.Core
{
	public static class Config
	{
		public static String Origin { get; set; }

		private static String _repoName;
		public static String RepoName
		{
			get { return _repoName; }
            set { _repoName = value;/*Char.ToUpper(value[0]) + value.Substring(1);*/ }
		}

		public static FileInfo SqlMetal { get; set; }
		public static Regex Test { get; set; }
		public static Regex Artifact { get; set; }
		public static Regex Cs { get; set; }

		public static String[] Databases { get; set; }
		public static String[] Roots { get; set; }

		public static String Message { get; set; }
		public static String Request { get; set; }
		public static String Command { get; set; }
		public static String Query { get; set; }

		public static String[] Products { get; set; }
		public static String[] Regions { get; set; }
		public static String[] Suffixes { get; set; }

		public static Framework Api { get; set; }
		public static Framework CsApi { get; set; }
		public static Framework Msmq { get; set; }
		public static Framework Db { get; set; }
		public static Framework Enums { get; set; }


	    public static readonly String[] Repos = new[]
	                                                {
	                                                    "ops",
	                                                    "payments",
	                                                    "risk",
	                                                    "technicalcomponents",
	                                                    "comms",
	                                                    "bi",
                                                        "marketing"
	                                                };

		static Config()
		{
			Origin = GetDefaultOrigin();
			//SqlMetal = new FileInfo(Path.Combine(Repo.Lib.FullName, "SqlMetal\\SqlMetal.exe"));
			Test = new Regex(@"\.Tests?(\.|$)", RegexOptions.IgnoreCase);
			Artifact = new Regex(@"bin|obj");
			Cs = new Regex(@"\.Csapi\.", RegexOptions.IgnoreCase);

			Message = "NServiceBus.IMessage";
			Request = "Wonga.Api.IApiRequest";
			Command = "Wonga.Api.Command";
			Query = "Wonga.Api.IQuery";

			Roots = new[] { "src", "components" };
			Products = new[] { "Wb", "PayLater" };
			Regions = new[] { "Uk", "Za", "Ca", "Pl" };
			Suffixes = new[] { "Command", "Query", "Event", "Message", "Csapi" };

			Api = new Framework { Project = String.Format("{0}.Api", Framework.Solution), Base = "ApiRequest", Folder = "Requests" };
			CsApi = new Framework { Project = String.Format("{0}.Cs", Framework.Solution), Base = "CsRequest", Folder = "Requests" };
			Msmq = new Framework { Project = String.Format("{0}.Msmq", Framework.Solution), Base = "MsmqMessage", Folder = "Messages" };
			Db = new Framework { Project = String.Format("{0}.Db", Framework.Solution), Base = "DbEntity", Folder = "Databases" };
			Enums = new Framework { Project = String.Format("{0}.Core", Framework.Solution), Folder = "Enums" };
		}

		private static String GetDefaultOrigin()
		{
			return @"..\v3split";
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
