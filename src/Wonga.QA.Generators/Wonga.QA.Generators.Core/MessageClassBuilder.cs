using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace Wonga.QA.Generators.Core
{
	public static class MessageClassBuilder
	{
		
		public enum MessageClassType
		{
			Api = 0,
			CsApi = 1,
			Msmq = 2
		}
		private static Dictionary<String, Type> _types;
		private static MessageClassType _messageClassType;
		

		public static List<MessageClassDefinition> Build(MessageClassType messageClassType, Dictionary<String, IEnumerable<XmlSchemaElement>> namespaceMessagePairs, Dictionary<String, Type> types, DirectoryInfo directoryInfo)
		{
			_messageClassType = messageClassType;
			_types = types;

			var classes = new List<MessageClassDefinition>(); 

			foreach (var element in namespaceMessagePairs)
				classes.AddRange(Build(element));

			return classes;
		}

		private static IEnumerable<MessageClassDefinition> Build(KeyValuePair<String, IEnumerable<XmlSchemaElement>> elementGroup)
		{
			var classNamespace = GetNamespace(elementGroup.Key);
			var messageType = classNamespace.Contains("Queries") ? "Query" : "Command";
			var region = GetRegionFromNamespace(classNamespace);
			foreach (var xmlSchemaElement in elementGroup.Value)
			{
				var className = GetClassName(xmlSchemaElement, messageType, region);
				var classBody = GenerateClassBody(className, classNamespace, xmlSchemaElement).ToString();
				yield return new MessageClassDefinition(classNamespace, className, classBody);
			}
		}

		private static String GetNamespace(String namespaceInV3)
		{
			switch (_messageClassType)
			{
				case MessageClassType.Api:
					{
						return GetNamespaceForApi(namespaceInV3);
					}
				case MessageClassType.CsApi:
					{
						return GetNamespaceForCsApi(namespaceInV3);
					}
				case MessageClassType.Msmq:
					{
						return GetNamespaceForMsmq(namespaceInV3);
					}
				default:
					{
						throw new NotImplementedException();
					}
			}
		}

		private static String GetNamespaceForApi(String namespaceInV3)
		{
			return "Wonga.QA.Framework.Api." + namespaceInV3;
		}

		private static String GetNamespaceForCsApi(String namespaceInV3)
		{
			return "Wonga.QA.Framework.Cs." + namespaceInV3.Replace("Wonga.", String.Empty).Replace("Csapi.", String.Empty);
		}

		private static String GetNamespaceForMsmq(String namespaceInV3)
		{
			throw new NotImplementedException();
		}

		private static String GetRegionFromNamespace(String classNamespace)
		{
			var regions = new[] {"Ca", "Pl", "Uk", "Za"};

			foreach (var r in regions.Where(classNamespace.Contains))
				return r;

			return String.Empty;
		}

		private static string GetClassName(XmlSchemaElement element, String messageType, String region)
		{
			return String.Format("{0}{1}{2}", element.Name, region, messageType );
		}

		private static StringBuilder GenerateClassBody(String className, String classNamespace, XmlSchemaElement element)
		{
			var classBuilder = CreateClassDefinitionHeader(className, classNamespace, element.Name);
			classBuilder = CreateClassDefinitionProperties(classBuilder, element);
			classBuilder = CreateClassDefinitionFooter(classBuilder);

			return classBuilder;
		}

		private static StringBuilder CreateClassDefinitionHeader(String className, String classNamespace, String elementName)
		{
			StringBuilder builder = new StringBuilder().AppendFormatLine(new[]
    		                                                              	{
																				"using System;",
    		                                                              		"using System.Xml.Serialization;",
																				"using Wonga.QA.Framework.Api;",
																				"",
																				"namespace {0}",
    		                                                              		"{{",
    		                                                              		"	[XmlRoot({1})]",
    		                                                              		"	public partial class {2} : ApiRequest<{2}>",
    		                                                              		"	{{",
    		                                                              	},
																		  classNamespace,
																		  elementName.Quote(),
																		  className);
			return builder;
		}

		private static StringBuilder CreateClassDefinitionProperties(StringBuilder classBuilder, XmlSchemaElement element)
		{
			IEnumerable<PropertyInfo> properties;

			try
			{
				properties = _types["Messages" + element.Name].GetProperties().Where(p => !p.IsIgnore());
			}
			catch (KeyNotFoundException e)
			{
				Console.WriteLine("No properties found for " + element.Name);
				return classBuilder;
			}

			foreach (PropertyInfo property in properties)
				classBuilder.AppendFormatLine(
					"		public Object {0} {{ get; set; }}", property.GetName());

			return classBuilder;
		}

		private static StringBuilder CreateClassDefinitionFooter(StringBuilder classBuilder)
		{
			return classBuilder.AppendLine("	}").AppendLine("}");
		}
	}
}
