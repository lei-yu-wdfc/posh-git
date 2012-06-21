using System;
using System.Collections.Generic;
using System.IO;

namespace Wonga.QA.Generators.Core
{
	public class GeneratorRepoDirectories
	{
		public const string DefaultEnumsDirectoryName = "Enums";
		public const string DefaultCodeDirectoryName = "Code";

		public DirectoryInfo ClassesDirectory { get; private set; }

		public DirectoryInfo CodeDirectory { get; private set; }

		public DirectoryInfo EnumsDirectory { get; private set; }


		public GeneratorRepoDirectories(string classesDirectoryName, 
										string codeDirectoryName = DefaultCodeDirectoryName, 
										string enumsDirectoryName = DefaultEnumsDirectoryName)
			:this(
				Repo.Directory(classesDirectoryName),
				SafeCreateRepoDirectory(codeDirectoryName),
				SafeCreateRepoDirectory(enumsDirectoryName))
		{
			
		}

		private GeneratorRepoDirectories(DirectoryInfo classesDirectory, DirectoryInfo codeDirectory, DirectoryInfo enumsDirectory)
		{
			ClassesDirectory = classesDirectory;
			CodeDirectory = codeDirectory;
			EnumsDirectory = enumsDirectory;
		}

		private static DirectoryInfo SafeCreateRepoDirectory(string directoryName)
		{
			return
				string.IsNullOrEmpty(directoryName)
					? null
					: Repo.Directory(directoryName);
		}
	}
}
