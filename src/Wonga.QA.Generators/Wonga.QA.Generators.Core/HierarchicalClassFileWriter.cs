using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wonga.QA.Generators.Core
{
	public static class HierarchicalClassFileWriter
	{
		private static DirectoryInfo _parentDirectory;
		private static DirectoryInfo _commandsDirectory;
		private static DirectoryInfo _queriesDirectory;

		private static readonly String[] Regions = new [] {"Ca", "Pl", "Uk", "Za"};

		public static void WriteClassFilesToDisk(DirectoryInfo parentDirectory, List<MessageClassDefinition> classes )
		{
			WriteFiles(_commandsDirectory, classes);
		}

		private static void CreateDirectoryIfDoesntExist(DirectoryInfo directory)
		{
			if( !Directory.Exists(directory.FullName))
			{
				Directory.CreateDirectory(directory.FullName);
			}
		}

		private static void WriteFiles(DirectoryInfo parentDirectory, List<MessageClassDefinition> classes)
		{
			foreach (var c in classes)
			{
				WriteFile(parentDirectory, c);
			}
		}

		private static void WriteFile(DirectoryInfo parentDirectory, MessageClassDefinition messageClassDefinition)
		{
			var fileFullName = GetDirectory(parentDirectory, messageClassDefinition);
			var file = new FileInfo(fileFullName);

			using (StreamWriter writer = file.CreateText())
				writer.Write(messageClassDefinition.ClassBody);
		}

		private static String GetDirectory(DirectoryInfo parentDirectory, MessageClassDefinition messageClassDefinition)
		{
			return Path.Combine(parentDirectory.FullName, messageClassDefinition.Component, messageClassDefinition.MessageType, messageClassDefinition.Region, messageClassDefinition.FileName);
		}
	}
}
