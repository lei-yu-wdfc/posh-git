using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.CSharp;

namespace Wonga.QA.Generators.Core
{
	public static class ClassBuilder
	{
		private static Dictionary<String, Type> _types;
		
		public static List<MessageClassDefinition> Build(Dictionary<String, IEnumerable<XmlSchemaElement>> elementGroups, Dictionary<String, Type> types, DirectoryInfo directoryInfo)
		{
			_types = types;

			var classes = new List<MessageClassDefinition>(); 

			foreach (var element in elementGroups)
				classes.AddRange(Build(element));

			return classes;
		}

		private static IEnumerable<MessageClassDefinition> Build(KeyValuePair<String, IEnumerable<XmlSchemaElement>> elementGroup)
		{
			var classNamespace = elementGroup.Key;
			var messageType = classNamespace.Contains("Queries") ? "Query" : "Command";
			var region = GetRegionFromNamespace(classNamespace);
			foreach (var xmlSchemaElement in elementGroup.Value)
			{
				var className = GetClassName(xmlSchemaElement, messageType, region);
				var classBody = GenerateClassBody(className, classNamespace, xmlSchemaElement).ToString();
				yield return new MessageClassDefinition(classNamespace, className, classBody);
			}
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
																				"",
																				"namespace {0}",
    		                                                              		"{{",
    		                                                              		"	[XmlRoot({1})]",
    		                                                              		"	public class {2} : ApiRequest<{2}>",
    		                                                              		"	{{",
    		                                                              	},
																		  classNamespace,
																		  elementName,
																		  className);
			var va = builder.ToString();
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
