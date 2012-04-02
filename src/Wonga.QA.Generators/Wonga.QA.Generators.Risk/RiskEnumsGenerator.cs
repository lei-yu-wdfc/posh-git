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
			GenerateTestMasks();
		}

		private static void GenerateTestMasks()
		{
			var riskQAConfig = GetRiskQAConfig();

			//TODO: Change file location to somewhere in the QAF.
			var riskEnumFile = Repo.File("RiskMasks.cs", new DirectoryInfo(@"c:\temp"));

			var stringBuilder = new StringBuilder().AppendFormatLine(new[]{
						"using System;",
						"",
						"namespace {0}.Temp",
						"{{",
						"	public enum RiskCheckpointMask",
						"	{{",
					}, Config.Api.Project);


			foreach (var checkpointMask in riskQAConfig.Elements("checkpointMasks").Elements("checkpointMask").Elements("mask"))
			{
				stringBuilder.AppendFormatLine("\t\t{0},", checkpointMask.Value);
			}

			stringBuilder.AppendLine("	}");
			stringBuilder.AppendLine("}");

			using (var streamWriter = riskEnumFile.CreateText())
			{
				streamWriter.Write(stringBuilder);
			}
		}

		private static DirectoryInfo RiskSrc
		{
			get { return Origin.Src.GetDirectories("Risk").Single(); }
		}

		private static	XElement GetRiskQAConfig()
		{
			var riskQAFolder = RiskSrc.GetDirectories("Wonga.Risk.Workflow.QA", SearchOption.AllDirectories).Single();

			var configXmlFile = riskQAFolder.GetFiles("QAConfig.xml").Single();

			return XElement.Load(configXmlFile.FullName);
		}

	
	}
}
