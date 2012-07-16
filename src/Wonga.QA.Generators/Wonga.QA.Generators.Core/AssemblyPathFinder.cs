
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wonga.QA.Generators.Core
{
	//Takes the name of a project and searches the package folder for the corresponding assembly
	public static class AssemblyPathFinder
	{
		private const String DefaultPackage = "CL";
		private static readonly String[] AlternativePackages = new[]{"WB"};

		private const String DevDeployment = "Dev";

		private const String DefaultRegion = "UK";
		private static readonly String[] Regions = new[] { "CA", "PL", "ST_UK", "UK", "ZA" };

		public static DirectoryInfo GetPath( FileInfo project, DirectoryInfo rootDirectory)
		{
			var projectName = project.GetFileNameWithoutExtension();
			var assemblyName = projectName + ".dll";
			var roughshotPath = GetRoughshotPath(projectName, rootDirectory);

			var path = GetPath(assemblyName, roughshotPath);
			return path;
		}

        public static FileInfo[] GetFiles(FileInfo project, DirectoryInfo rootDirectory)
        {
            var projectName = project.GetFileNameWithoutExtension();
            var assemblyName = projectName + ".dll";
            return rootDirectory.GetFiles(assemblyName, SearchOption.AllDirectories).ToArray();
        }

		private static String GetRoughshotPath(String projectName, DirectoryInfo packageRoot)
		{
			var roughshotPath = new StringBuilder();

			var package = GetPackage(projectName);
			roughshotPath.Append(String.Format("\\{0}\\", package));

			var deployment = DevDeployment;
			roughshotPath.Append(String.Format("{0}\\", deployment));

			var region = GetRegion(projectName);
			roughshotPath.Append(String.Format("{0}\\", region));

			return packageRoot + roughshotPath.ToString();
		}

		private static DirectoryInfo GetPath(String assemblyName, String topDirectory)
		{
			var files = Directory.GetFiles(topDirectory, assemblyName, SearchOption.AllDirectories);

			if( files.Any())
			{
				var directory = files.FirstOrDefault();
				directory = Regex.Replace(directory, assemblyName, String.Empty, RegexOptions.IgnoreCase);
				return new DirectoryInfo(directory);
			}

			return null;
		}

		private static String GetPackage(String projectName)
		{
			foreach (var p in AlternativePackages.Where(r => projectName.IndexOf(String.Format(".{0}", r), StringComparison.OrdinalIgnoreCase) >= 0))
				    return p.ToUpper();

			return DefaultPackage;
		}

		private static String GetRegion(String projectName)
		{
			foreach (var r in Regions)
			{
				var suffix = projectName.Split('.').Last();
				if(suffix.Equals(r, StringComparison.OrdinalIgnoreCase))
					return r.ToUpper();
			}

			return DefaultRegion;
		}
	}
}
