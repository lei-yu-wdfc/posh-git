﻿using System;
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


		public string ClassesDirectoryName { get; private set; }

		public string CodeDirectoryName { get; private set; }

		public string EnumsDirectoryName { get; private set; }


		public GeneratorRepoDirectories(string classesDirectoryName,
										string codeDirectoryName = DefaultCodeDirectoryName,
										string enumsDirectoryName = DefaultEnumsDirectoryName)
		{
			ClassesDirectoryName = classesDirectoryName;
			CodeDirectoryName = codeDirectoryName;
			EnumsDirectoryName = enumsDirectoryName;

			ClassesDirectory = Repo.Directory(ClassesDirectoryName);
			CodeDirectory = SafeCreateRepoDirectory(CodeDirectoryName);
			EnumsDirectory = SafeCreateRepoDirectory(EnumsDirectoryName);
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