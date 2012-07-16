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

		public static IEnumerable<Assembly> GetAssemblies()
		{
			var proj = GetProjects();
			var assemblyFiles = (from fileInfo in proj let path = AssemblyPathFinder.GetPath(fileInfo, Build) where path != null select fileInfo.GetAssembly(path));

			return assemblyFiles.Select(f => Assembly.LoadFrom(f.FullName)).Distinct();
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

		public static IEnumerable<FileInfo> GetSchemas()
		{
			return Src.GetFiles("Wonga.*.xsd", SearchOption.AllDirectories).Where(f => !f.IsTest() && !f.IsArtifact());
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
