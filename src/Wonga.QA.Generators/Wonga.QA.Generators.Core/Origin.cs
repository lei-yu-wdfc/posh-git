using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Wonga.QA.Generators.Core
{
	public static class Origin
	{
		public static DirectoryInfo Root
		{
			get { return new DirectoryInfo(Path.Combine(Repo.Root.FullName, Config.Origin)); }
		}

		public static DirectoryInfo Src
		{
			get { return new DirectoryInfo(Path.Combine(Root.FullName, Config.RepoName, "src")); }
		}

		public static DirectoryInfo Build
		{
			get { return new DirectoryInfo(Path.Combine(Root.FullName, "package")); }
		}

		public static DirectoryInfo WwwRootDirectory
		{
			get { return new DirectoryInfo(@"C:\inetpub\wwwroot");}
		}

		public static DirectoryInfo ApiDirectory
		{
			get { return new DirectoryInfo(Path.Combine(WwwRootDirectory.FullName, "API")); }
		}

		public static DirectoryInfo UcgDirectory
		{
			get { return new DirectoryInfo(Path.Combine(WwwRootDirectory.FullName, "UCG")); }
		}

		public static DirectoryInfo CsApiDirectory
		{
			get { return new DirectoryInfo(Path.Combine(WwwRootDirectory.FullName, "CSAPI")); }
		}

		public static IEnumerable<Assembly> GetAssemblies()
		{
			var proj = GetProjects();
			var assemblyFiles = (from fileInfo in proj let path = AssemblyPathFinder.GetPath(fileInfo, Build) where path != null select fileInfo.GetAssembly(path));
            
			return assemblyFiles.Where(x => x != null).Select(f => Assembly.LoadFrom(f.FullName)).Distinct();
		}

		public static IEnumerable<FileInfo> GetProjects()
		{
			return Src.GetFiles("Wonga.*.csproj", SearchOption.AllDirectories).Where(f => !f.IsTest());
		}

		public static IEnumerable<Type> GetTypes()
		{
			var assemblies = GetAssemblies().ToList();

			var types = new List<Type>();

			foreach (var assembly in assemblies)
			{
				try
				{
					types.AddRange(assembly.GetTypes().Distinct());
				}
				catch (ReflectionTypeLoadException ex)
				{
					LogAssemblyLoaderExceptions(assembly, ex);
				}
			}

			return types;
		}

		public static FileInfo GetApiCommandsSchema()
		{
			var dir = new DirectoryInfo(Path.Combine(ApiDirectory.FullName, "commands"));
			return dir.GetFiles("Api.xsd", SearchOption.AllDirectories).First();
		}

		public static FileInfo GetApiQueriesSchema()
		{
			var dir = new DirectoryInfo(Path.Combine(ApiDirectory.FullName, "queries"));
			return dir.GetFiles("Api.xsd", SearchOption.AllDirectories).First();
		}

		public static FileInfo GetCsApiCommandsSchema()
		{
			var dir = new DirectoryInfo(Path.Combine(CsApiDirectory.FullName, "commands"));
			return dir.GetFiles("Api.xsd", SearchOption.AllDirectories).First();
		}

		public static FileInfo GetCsApiQueriesSchema()
		{
			var dir = new DirectoryInfo(Path.Combine(CsApiDirectory.FullName, "queries"));
			return dir.GetFiles("Api.xsd", SearchOption.AllDirectories).First();
		}

		private static void LogAssemblyLoaderExceptions(Assembly assembly, ReflectionTypeLoadException ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;

			foreach (var lx in ex.LoaderExceptions)
				Console.WriteLine(String.Format("{0} at directory {1} which is a dependency for {2}", lx.Message, assembly.Location, assembly.FullName));

			Console.ResetColor();
		}
	}
}
