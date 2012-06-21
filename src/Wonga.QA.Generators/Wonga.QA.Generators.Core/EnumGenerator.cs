using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

		public EnumGenerator(bool continueOnError)
			: this(EnumGenerationMode.IncludeConstantValues, DefaultEnumFullNameStartFilter, continueOnError)
		{
			
		}

		public EnumGenerator(EnumGenerationMode mode = EnumGenerationMode.IncludeConstantValues, string filter = DefaultEnumFullNameStartFilter, bool continueOnError = true)
		{
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

		//tODO: remove enum path stuff
		/// <summary>
		/// generates all enumeration members of a class
		/// </summary>
		/// <param name="classMemberTypes">all the member types of the class</param>
		/// <param name="generatedEnumNamespace">the new namespace for the generated enums</param>
		/// <param name="enumRootDirectory">root directory to place the enum files</param>
		/// <param name="subfolderName">subdirectory to place all the enums used by this class</param>
		public void GenerateAllEnumsUsedByClass(IEnumerable<Type> classMemberTypes, string generatedEnumNamespace, DirectoryInfo enumRootDirectory, string subfolderName)
		{
			foreach (Type classMemberType in classMemberTypes)
			{
				GenerateAllEnumsUsedByClassMember(classMemberType, generatedEnumNamespace, enumRootDirectory, subfolderName);
			}
		}

		//TODO: delete this!!!!
		/// <summary>
		/// generates all enumeration used my a member of a class
		/// </summary>
		/// <param name="classMemberType">member type of the class</param>
		/// <param name="generatedEnumNamespace">the new namespace for the generated enums</param>
		/// <param name="enumRootDirectory">root directory to place the enum files</param>
		/// <param name="subfolderName">subdirectory to place all the enums used by this class</param>
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
							
							AddToCurrentClassGeneratedEnumsIfUnique(enumGenerationResult.EnumDefinition);
							break;

						case EnumGenerationStatus.NotGenerated:

							AddToCurrentClassGeneratedEnumsIfUnique(enumGenerationResult.EnumDefinition);
							GenerateEnum(messageMemberEnumType, enumGenerationResult.EnumDefinition.GeneratedNamespace, enumRootDirectory, subfolderName);
							AddNewGeneratedEnumDefinition(enumGenerationResult);
							break;
					}
					
				}
				catch (Exception e)
				{
					ErrorsOccurred = true;
					Console.Error.WriteLine("\t*** FAILED GENERATION FOR ENUM: {0}. {1}", messageMemberEnumType.GetName(), e.Message);
					if(!ContinueOnError)
					{
						throw;
					}
				}
			}
		}

		public void GenerateAllEnumsUsedByClassMember(Type classMemberType, DirectoryInfo enumRootDirectory)
		{
			var messageMemberEnumTypes = GetCustomEnumTypesUsedByClassMember(classMemberType);

			foreach (Type messageMemberEnumType in messageMemberEnumTypes)
			{
				try
				{

					string enumNamespaceRelativePath = string.IsNullOrEmpty(messageMemberEnumType.Namespace)
					                                   	? string.Empty
					                                   	: messageMemberEnumType.Namespace.Replace("Wonga.", string.Empty);
							
					string enumSubfolderName = enumNamespaceRelativePath.Replace(".", new string(Path.DirectorySeparatorChar, 1));

					//TODO: fix this!!!!! pass the FW ????? ON THE 2 PLACES
					string generatedEnumNamespace =
						string.IsNullOrEmpty(enumNamespaceRelativePath) 
						? string.Format("{0}.{1}", Config.Api.Project, Config.Enums.Folder)
						: string.Format("{0}.{1}.{2}", Config.Api.Project, Config.Enums.Folder, enumNamespaceRelativePath);
					
					EnumGenerationResult enumGenerationResult = GetEnumGenerationResult(messageMemberEnumType, generatedEnumNamespace);
					switch (enumGenerationResult.Status)
					{
						case EnumGenerationStatus.UnableToGenerate:

							throw new NotImplementedException(string.Format("Can not generate enumeration {0}", messageMemberEnumType.FullName));

						case EnumGenerationStatus.AlreadyGenerated:

							AddToCurrentClassGeneratedEnumsIfUnique(enumGenerationResult.EnumDefinition);
							break;

						case EnumGenerationStatus.NotGenerated:

							AddToCurrentClassGeneratedEnumsIfUnique(enumGenerationResult.EnumDefinition);
							GenerateEnum(messageMemberEnumType, enumGenerationResult.EnumDefinition.GeneratedNamespace, enumRootDirectory, enumSubfolderName);
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

		private void GenerateEnum(Type enumType, string generatedEnumNamespace, DirectoryInfo rootEnumDirectory, string subfolderName)
		{
			String enumName = enumType.GetName().ToEnum().ToCamel();

			DirectoryInfo enumDirectory = Repo.Directory(subfolderName, rootEnumDirectory);
			FileInfo fenum = Repo.File(String.Format("{0}.cs", enumName), enumDirectory);

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
