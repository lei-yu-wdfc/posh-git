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

		void GenerateAllEnumsUsedByClass(IEnumerable<Type> classMemberTypes, DirectoryInfo enumRootDirectory);

		void GenerateAllEnumsUsedByClassMember(Type classMemberType, DirectoryInfo enumRootDirectory);

		string GetEnumUsingDirectivesForCurrentClass();
	}
}