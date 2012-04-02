using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Risk
{
	public class RiskEnumsGenerator
	{
		public static void Main(string[] args)
		{
            if (args.Any())
                Config.Origin = args.Single();

			GenerateTestMasks();
		}

		private static void GenerateTestMasks()
		{
			var riskQaConfig = GetRiskQaConfig();

            //We cannot use the same project as Api Generator since the old values are removed when that is run.
            var riskEnumFile = Repo.File("NewRiskMasks.cs", Repo.Directory("Enums"));

			var stringBuilder = new StringBuilder().AppendFormatLine(new[]{
						"using System;",
						"",
						"namespace {0}.Temp",
						"{{",
						"	public enum RiskCheckpointMask",
						"	{{",
					}, Config.Api.Project);


			foreach (var checkpointMask in riskQaConfig.Elements("checkpointMasks").Elements("checkpointMask").Elements("mask"))
			{
				stringBuilder.AppendFormatLine("\t\t{0},", checkpointMask.Value);
			}

			stringBuilder.AppendLine("	}");
			stringBuilder.AppendLine("}");

			using (var streamWriter = riskEnumFile.CreateText())
			{
				streamWriter.Write(stringBuilder);
			}

            //We will then use something like : Repo.Insert -> Need to discuss where Risk things go 
		}

		private static DirectoryInfo RiskSrc
		{
			get { return Origin.Src.GetDirectories("Risk").Single(); }
		}
		private static	XElement GetRiskQaConfig()
		{
			var riskQaFolder = RiskSrc.GetDirectories("Wonga.Risk.Workflow.QA", SearchOption.AllDirectories).Single();

			var configXmlFile = riskQaFolder.GetFiles("QAConfig.xml").Single();

			return XElement.Load(configXmlFile.FullName);
		}
	
	}
}
