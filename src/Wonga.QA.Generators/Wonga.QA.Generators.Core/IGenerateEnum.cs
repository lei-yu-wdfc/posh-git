using System;
using System.Collections.Generic;
using System.IO;

namespace Wonga.QA.Generators.Core
{
	public interface IGenerateEnum
	{
		List<GeneratedEnumDefinition> CurrentClassGeneratedEnums { get; }

		bool ContinueOnError { get; }

		EnumGenerationMode Mode { get; }

		bool ErrorsOccurred { get; }

		string EnumFullNameStartFilter { get; }

		void StartEnumGenerationForClass(string classGeneratedNamespace);

		void GenerateAllEnumsUsedByClass(IEnumerable<Type> classMemberTypes, string generatedEnumNamespace,
		                                 DirectoryInfo enumRootDirectory, string subfolderName);

		void GenerateAllEnumsUsedByClassMember(Type classMemberType, string generatedEnumNamespace,
		                                       DirectoryInfo enumRootDirectory, string subfolderName);

		string GetEnumUsingDirectivesForCurrentClass();
	}
}