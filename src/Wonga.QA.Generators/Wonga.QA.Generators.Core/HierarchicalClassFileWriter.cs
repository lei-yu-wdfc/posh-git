using System;
using System.Collections.Generic;
using System.IO;

namespace Wonga.QA.Generators.Core
{
	public static class HierarchicalClassFileWriter
	{
		public static void WriteClassFilesToDisk(DirectoryInfo parentDirectory, List<MessageClassDefinition> classes )
		{
			WriteFiles(parentDirectory, classes);
		}

		private static void CreateDirectoryIfDoesntExist(String directory)
		{
			if( !Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
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
			var fileDirectory = GetDirectory(parentDirectory, messageClassDefinition);
			CreateDirectoryIfDoesntExist(fileDirectory);

			var file = new FileInfo(Path.Combine(fileDirectory, messageClassDefinition.FileName));
			DeleteFileIfExists(file);

			using (StreamWriter writer = file.CreateText())
				writer.Write(messageClassDefinition.ClassBody);
		}

		private static String GetDirectory(DirectoryInfo parentDirectory, MessageClassDefinition messageClassDefinition)
		{
			var typeFolder = messageClassDefinition.MessageType == "Command" ? "Commands" : "Queries";
			var componentFolder = GetSubDirectoryFromComponent(messageClassDefinition.Component);
			return Path.Combine(parentDirectory.FullName, 
				componentFolder,
				typeFolder, 
				messageClassDefinition.Region);
		}

		private static String GetSubDirectoryFromComponent(String component)
		{
			if(!component.Contains("."))
				return component;

			var parts = component.Split('.');
			return Path.Combine(parts);
		}

		private static void DeleteFileIfExists(FileInfo file)
		{
			if(File.Exists(file.FullName))
			{
				File.Delete(file.FullName);
			}
		}
	}
}
