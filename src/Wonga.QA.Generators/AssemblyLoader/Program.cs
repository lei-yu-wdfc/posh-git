using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyLoader
{
	class Program
	{
		static void Main(string[] args)
		{
			var dir = Directory.GetParent(Directory.GetCurrentDirectory());

			var files = dir.GetFiles("Wonga.*.dll", SearchOption.AllDirectories);

			foreach (var fileInfo in files)
			{
				LoadAssembly(fileInfo);
			}
			Console.WriteLine("Press a key");
			Console.ReadKey();
		}

		private static void LoadAssembly(FileInfo fileInfo)
		{
			try
			{
				var assembly = Assembly.LoadFrom(fileInfo.FullName);
				assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				string loaderEx = "";
				ex.LoaderExceptions.ForEach(x => { loaderEx += x.Message + "\n"; });
				Console.WriteLine(string.Format("{0} \n {1}", ex.Message, loaderEx));
			}
		}
	}

	public static class Extensions
	{
		public static List<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			List<T> list = enumerable.ToList();
			list.ForEach(action);
			return list;
		}
	}
}
