using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wonga.QA.Generators.Core
{
	public class EnumGenerator //: IGenerateEnums ????
	{
		private readonly Dictionary<String, GeneratedEnumDefinition>  _allGeneratedEnumDefinitions;

		public List<string> CurrentClassGeneratedEnumNamespaces { get; private set; }

		public bool ContinueOnError { get; set; }

		public EnumGenerator(bool continueOnError = true)
		{
			ContinueOnError = continueOnError;
			_allGeneratedEnumDefinitions = new Dictionary<string, GeneratedEnumDefinition>();
			CurrentClassGeneratedEnumNamespaces = new List<string>();
		}

		public void StartEnumGenerationForClass()
		{
			CurrentClassGeneratedEnumNamespaces = new List<string>();
		}

		public void GenerateAllEnumsUsedByClass(IEnumerable<Type> classMemberTypes, string generatedEnumNamespace, DirectoryInfo enumRootDirectory, string subfolderName)
		{
			foreach (Type classMemberType in classMemberTypes)
			{
				GenerateAllEnumsUsedByClassMember(classMemberType, generatedEnumNamespace, enumRootDirectory, subfolderName);
			}
		}

		public void GenerateAllEnumsUsedByClassMember(Type classMemberType, string generatedEnumNamespace, DirectoryInfo enumRootDirectory, string subfolderName)
		{
			var messageMemberEnumTypes = GetCustomEnumTypesUsedByClassMember(classMemberType);

			foreach (Type messageMemberEnumType in messageMemberEnumTypes)
			{
				try
				{
					EnumGenerationResult enumGenerationResult = GetEnumGenerationResult(messageMemberEnumType, generatedEnumNamespace);
					switch (enumGenerationResult.Status)
					{
						case EnumGenerationStatus.UnableToGenerate:

							throw new NotImplementedException(string.Format("Can not generate enumeration {0}", messageMemberEnumType.FullName));

						case EnumGenerationStatus.AlreadyGenerated:
							
							AddToCurrentClassGeneratedEnumNamespacesIfUnique(enumGenerationResult.EnumDefinition.GeneratedNamespace);
							break;

						case EnumGenerationStatus.NotGenerated:

							AddToCurrentClassGeneratedEnumNamespacesIfUnique(enumGenerationResult.EnumDefinition.GeneratedNamespace);
							GenerateEnum(messageMemberEnumType, enumGenerationResult.EnumDefinition.GeneratedNamespace, enumRootDirectory, subfolderName);
							AddNewGeneratedEnumDefinition(enumGenerationResult);
							break;
					}
					
				}
				catch (Exception e)
				{
					Console.WriteLine("\t*** FAILED GENERATION FOR ENUM: {0}. {1}", messageMemberEnumType.GetName(), e.Message);
					if(!ContinueOnError)
					{
						throw;
					}
				}
			}
		}

		private void AddNewGeneratedEnumDefinition(EnumGenerationResult enumGenerationResult)
		{
			//add the new generated enum to the dictionaty
			_allGeneratedEnumDefinitions.Add(
				enumGenerationResult.EnumDefinition.OriginalFullName,
			    enumGenerationResult.EnumDefinition);
		}

		private void AddToCurrentClassGeneratedEnumNamespacesIfUnique(string generatedNamespace)
		{
			//do not have repeated enum definitions for each class
			if (!CurrentClassGeneratedEnumNamespaces.Contains(generatedNamespace))
			{
				CurrentClassGeneratedEnumNamespaces.Add(generatedNamespace);
			}
		}

		//DO WE ACTUALLY NEED THIS... is enum should be enough....
		private static IEnumerable<Type> GetCustomEnumTypesUsedByClassMember(Type classMemberType)
		{
			var messageEnumMembers = new List<Type>();
			if (classMemberType.IsEnum)
				messageEnumMembers.Add(classMemberType);

			if (classMemberType.IsGenericType)
				messageEnumMembers.AddRange(classMemberType.GetGenericArguments().Where(a => a.IsEnum));

			messageEnumMembers = messageEnumMembers.Where(t => t.FullName != null && t.FullName.StartsWith("Wonga")).ToList();
			return messageEnumMembers;
		}

		private EnumGenerationResult GetEnumGenerationResult(Type enumType, string generatedEnumNamespace)
		{
			var enumDefinition = new GeneratedEnumDefinition(enumType, generatedEnumNamespace);

			if (_allGeneratedEnumDefinitions.ContainsKey(enumDefinition.OriginalFullName))
			{
				if (_allGeneratedEnumDefinitions[enumDefinition.OriginalFullName].ValueNames.SequenceEqual(enumDefinition.ValueNames))
				{
					//use existing one
					return new EnumGenerationResult(EnumGenerationStatus.AlreadyGenerated, _allGeneratedEnumDefinitions[enumDefinition.OriginalFullName]);
				}

				return new EnumGenerationResult(EnumGenerationStatus.UnableToGenerate, enumDefinition);
			}

			return new EnumGenerationResult(EnumGenerationStatus.NotGenerated, enumDefinition);
		}

		private static void GenerateEnum(Type enumType, string generatedEnumNamespace, DirectoryInfo rootEnumDirectory, string subfolderName)
		{
			String enumName = enumType.GetName().ToEnum().ToCamel();

			DirectoryInfo enumDirectory = Repo.Directory(subfolderName, rootEnumDirectory);
			FileInfo fenum = Repo.File(String.Format("{0}.cs", enumName), enumDirectory);

			StringBuilder builder1 = new StringBuilder().AppendFormatLine(new[]
									                                        {
									                                            "namespace {0}",
									                                            "{{",
									                                            "    public enum {1}",
									                                            "    {{"
									                                        },
																		generatedEnumNamespace,
																		enumName);

			foreach (Object value in Enum.GetValues(enumType))
				builder1.AppendFormatLine("        {0} = {1},", value, (int)value);

			builder1
				.AppendLine("    }")
				.AppendLine("}");

			using (StreamWriter writer = fenum.CreateText())
				writer.Write(builder1);

			Console.WriteLine("\t{0} \u2192 {1}", enumType.Name, fenum.Name);
		}
	}
}
