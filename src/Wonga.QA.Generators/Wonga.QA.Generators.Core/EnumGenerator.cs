using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QAFramework = Wonga.QA.Generators.Core.Config.Framework;

namespace Wonga.QA.Generators.Core
{
	public class EnumGenerator : IGenerateEnum
	{
		public const string DefaultEnumFullNameStartFilter = "Wonga";

		private const string ValueGenerationStandardTemplate =	"        {0},";
		private const string ValueGenerationIncludeConstantTemplate = "        {0} = {1},";
		private const string DescriptionGenerationTemplate = "        [Description(\"{0}\")]";

		private readonly Dictionary<String, GeneratedEnumDefinition>  _allGeneratedEnumDefinitions;

		private string _currentClassGeneratedNamespace;

		/// <summary>
		/// returns the list of enums generated for the current class
		/// </summary>
		public List<GeneratedEnumDefinition> CurrentClassGeneratedEnums { get; private set; }

		/// <summary>
		/// if <code>true</code> generator ends imediatelly and throws exception
		/// if <code>false</code> generator prints error and continues generation
		/// </summary>
		public bool ContinueOnError { get; private set; }

		/// <summary>
		/// defines the way in which the enums will be generated
		/// </summary>
		public EnumGenerationMode Mode { get; private set; }

		/// <summary>
		/// if any errors occurred during the generation process
		/// </summary>
		public bool ErrorsOccurred { get; private set; }

		/// <summary>
		/// will only generate enums whose full name begins with this filter
		/// if empty or null no filter will be applied
		/// </summary>
		public string EnumFullNameStartFilter { get; private set; }

		/// <summary>
		/// target framework to which the enums will be generated
		/// </summary>
		public QAFramework TargetFramework { get; private set; }

		public EnumGenerator(QAFramework targetFramework, bool continueOnError)
			: this(targetFramework, EnumGenerationMode.IncludeConstantValues, DefaultEnumFullNameStartFilter, continueOnError)
		{
			
		}

		public EnumGenerator(QAFramework targetFramework, EnumGenerationMode mode = EnumGenerationMode.IncludeConstantValues, string filter = DefaultEnumFullNameStartFilter, bool continueOnError = true)
		{
			TargetFramework = targetFramework;
			ContinueOnError = continueOnError;
			Mode = mode;
			EnumFullNameStartFilter = filter;
			_allGeneratedEnumDefinitions = new Dictionary<string, GeneratedEnumDefinition>();
			CurrentClassGeneratedEnums = new List<GeneratedEnumDefinition>();
		}

		/// <summary>
		/// this method should be called when the generation process is started for a new class
		/// </summary>
		public void StartEnumGenerationForClass(string classGeneratedNamespace)
		{
			CurrentClassGeneratedEnums = new List<GeneratedEnumDefinition>();
			_currentClassGeneratedNamespace = classGeneratedNamespace;
		}

		/// <summary>
		/// generates all enumeration members of a class
		/// </summary>
		/// <param name="classMemberTypes">all the member types of the class</param>
		/// <param name="enumRootDirectory">root directory to place the enum files</param>
		public void GenerateAllEnumsUsedByClass(IEnumerable<Type> classMemberTypes, DirectoryInfo enumRootDirectory)
		{
			foreach (Type classMemberType in classMemberTypes)
			{
				GenerateAllEnumsUsedByClassMember(classMemberType, enumRootDirectory);
			}
		}

		/// <summary>
		/// generates all enumeration used my a member of a class
		/// </summary>
		/// <param name="classMemberType">member type of the class</param>
		/// <param name="enumRootDirectory">root directory to place the enum files</param>
		public void GenerateAllEnumsUsedByClassMember(Type classMemberType, DirectoryInfo enumRootDirectory)
		{
			var entityGenerator = new EntityGenerator(enumRootDirectory, TargetFramework.Project, Config.Enums.Folder);

			var messageMemberEnumTypes = GetCustomEnumTypesUsedByClassMember(classMemberType);

			foreach (Type messageMemberEnumType in messageMemberEnumTypes)
			{
				try
				{
					GeneratedEntityDefinition generatedEntityDefinition = entityGenerator.GenerateEntityDefinition(messageMemberEnumType);

					EnumGenerationResult enumGenerationResult = GetEnumGenerationResult(messageMemberEnumType, generatedEntityDefinition.Namespace);
					switch (enumGenerationResult.Status)
					{
						case EnumGenerationStatus.UnableToGenerate:

							throw new NotImplementedException(string.Format("Can not generate enumeration {0}", messageMemberEnumType.FullName));

						case EnumGenerationStatus.AlreadyGenerated:

							AddToCurrentClassGeneratedEnumsIfUnique(enumGenerationResult.EnumDefinition);
							break;

						case EnumGenerationStatus.NotGenerated:

							AddToCurrentClassGeneratedEnumsIfUnique(enumGenerationResult.EnumDefinition);
							GenerateEnum(messageMemberEnumType, enumGenerationResult.EnumDefinition.GeneratedNamespace, generatedEntityDefinition.Directory);
							AddNewGeneratedEnumDefinition(enumGenerationResult);
							break;
					}

				}
				catch (Exception e)
				{
					ErrorsOccurred = true;
					Console.Error.WriteLine("\t*** FAILED GENERATION FOR ENUM: {0}. {1}", messageMemberEnumType.GetName(), e.Message);
					if (!ContinueOnError)
					{
						throw;
					}
				}
			}
		}

		public string GetEnumUsingDirectivesForCurrentClass()
		{
			var usingDirectivesBuilder = new StringBuilder();

			foreach (string messageGeneratedEnumNamespace in EnumGenerationNamespacesForCurrentClass)
			{
				usingDirectivesBuilder.AppendFormatLine("using {0};", messageGeneratedEnumNamespace);
			}
			if (usingDirectivesBuilder.Length > 0)
			{
				usingDirectivesBuilder.AppendLine("");
			}

			return usingDirectivesBuilder.ToString();
		}

		private void AddNewGeneratedEnumDefinition(EnumGenerationResult enumGenerationResult)
		{
			//add the new generated enum to the dictionaty
			_allGeneratedEnumDefinitions.Add(
				enumGenerationResult.EnumDefinition.OriginalFullName,
			    enumGenerationResult.EnumDefinition);
		}

		private void AddToCurrentClassGeneratedEnumsIfUnique(GeneratedEnumDefinition enumDefinition)
		{
			if (!CurrentClassGeneratedEnums.Any(currentEnum => currentEnum.GeneratedNamespace == enumDefinition.GeneratedNamespace))
			{
				CurrentClassGeneratedEnums.Add(enumDefinition);
			}
		}

		private IEnumerable<Type> GetCustomEnumTypesUsedByClassMember(Type classMemberType)
		{
			var enumMembers = new List<Type>();
			if (classMemberType.IsEnum)
				enumMembers.Add(classMemberType);

			//this is needed for nullable enums or generics of enums
			if (classMemberType.IsGenericType)
				enumMembers.AddRange(classMemberType.GetGenericArguments().Where(a => a.IsEnum));

			if (!string.IsNullOrEmpty(EnumFullNameStartFilter))
			{
				enumMembers = enumMembers.Where(t => t.FullName != null && t.FullName.StartsWith(EnumFullNameStartFilter)).ToList();
			}
			return enumMembers;
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

		private void GenerateEnum(Type enumType, string generatedEnumNamespace, DirectoryInfo enumDirectory)
		{
			String enumName = enumType.GetName().ToEnum().ToCamel();

			FileInfo fenum = Repo.File(String.Format("{0}.cs", enumName), enumDirectory, true);

			var enumBuilder = CreateEnumBuilder(generatedEnumNamespace, enumName);

			AddAllEnumValues(enumBuilder, enumType);
			
			enumBuilder
				.AppendLine("    }")
				.AppendLine("}");

			using (StreamWriter writer = fenum.CreateText())
				writer.Write(enumBuilder);

			Console.WriteLine("\t{0} \u2192 {1}", enumType.Name, fenum.Name);
		}

		private StringBuilder CreateEnumBuilder(string generatedEnumNamespace, string enumName)
		{
			string[] enumBuilderTemplate =
				(Mode & EnumGenerationMode.IncludeDescription) == EnumGenerationMode.IncludeDescription
					? new[]
					  	{
					  		"using System.ComponentModel;",
					  		"namespace {0}",
					  		"{{",
					  		"    public enum {1}",
					  		"    {{"

					  	}
					: new[]
					  	{
					  		"namespace {0}",
					  		"{{",
					  		"    public enum {1}",
					  		"    {{"
					  	};

			return new StringBuilder().AppendFormatLine(
										enumBuilderTemplate,
										generatedEnumNamespace,
										enumName);
		}

		private void AddAllEnumValues(StringBuilder enumBuilder, Type enumType)
		{
			foreach (Object value in Enum.GetValues(enumType))
			{
				AddEnumValue(enumBuilder, enumType, value);
			}
		}

		private void AddEnumValue(StringBuilder enumBuilder, Type enumType, Object value)
		{
			string valueLine = null;
			string descriptionLine = null;
			string enumValueName = Enum.GetName(enumType, value);

			if(Mode == EnumGenerationMode.Standard)
			{
				valueLine = string.Format(ValueGenerationStandardTemplate, value);
			}
			else
			{
				if ((Mode & EnumGenerationMode.IncludeDescription) == EnumGenerationMode.IncludeDescription)
				{
					descriptionLine = string.Format(DescriptionGenerationTemplate, enumValueName);
				}
				if ((Mode & EnumGenerationMode.IncludeConstantValues) == EnumGenerationMode.IncludeConstantValues)
				{
					valueLine = string.Format(ValueGenerationIncludeConstantTemplate, GetEnumValueNameForMode(value, enumValueName), (int)value);
				}
				if ((Mode & EnumGenerationMode.UseNormalTypeName) == EnumGenerationMode.UseNormalTypeName)
				{
					valueLine = string.Format(ValueGenerationStandardTemplate, GetEnumValueNameForMode(value, enumValueName));
				}
			}

			if(!string.IsNullOrEmpty(descriptionLine))
			{
				enumBuilder.AppendLine(descriptionLine);
			}

			if(!string.IsNullOrEmpty(valueLine))
			{
				enumBuilder.AppendLine(valueLine);
			}
		}

		private string GetEnumValueNameForMode(object value, string valueName)
		{
			if ((Mode & EnumGenerationMode.UseNormalTypeName) == EnumGenerationMode.UseNormalTypeName)
			{
				return valueName.GetNormalTypeName();
			}

			return value.ToString();
		}

		private IEnumerable<string> EnumGenerationNamespacesForCurrentClass
		{
			get
			{
				return
					CurrentClassGeneratedEnums
						.Where(e => e.GeneratedNamespace != _currentClassGeneratedNamespace)
						.Select(e => e.GeneratedNamespace);
			}
		}
		
		
	}
}
