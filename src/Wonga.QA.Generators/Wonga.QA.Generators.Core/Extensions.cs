using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Wonga.QA.Generators.Core
{
	public static class Extensions
	{
		public static Boolean IsRoot(this DirectoryInfo directory)
		{
			return directory.EnumerateDirectories(".git").Any();
		}

		public static String GetFileNameWithoutExtension(this FileInfo file)
		{
			return Path.GetFileNameWithoutExtension(file.Name);
		}

		public static FileInfo ChangeExtension(this FileInfo file, String extension)
		{
			return new FileInfo(Path.ChangeExtension(file.FullName, extension));
		}

		public static IEnumerable<string> GetTree(this FileInfo file)
		{
			yield return file.Name;
			DirectoryInfo directory = file.Directory;
			while (directory != null && !directory.IsRoot())
			{
				yield return directory.Name;
				directory = directory.Parent;
			}
		}

		public static Boolean IsTest(this FileInfo file)
		{
			return file.GetTree().Any(Config.Test.IsMatch);
		}

		public static Boolean IsArtifact(this FileInfo file)
		{
			return file.GetTree().Any(Config.Artifact.IsMatch);
		}

		public static Boolean IsCs(this FileInfo file)
		{
			return Config.Cs.IsMatch(file.Name);
		}

		public static XmlSchema GetSchema(this FileInfo file)
		{
			using (XmlReader reader = XmlReader.Create(file.FullName))
				return XmlSchema.Read(reader, (s, a) => { throw a.Exception; });
		}

		public static IEnumerable<Type> GetTypes(this FileInfo file)
		{
			return Assembly.LoadFrom(file.FullName).GetTypes();
		}

		public static String GetAssembly(this FileInfo file)
		{
			XElement root = XDocument.Load(file.FullName).Root;
			return root.Descendants(root.GetDefaultNamespace().GetName("AssemblyName")).Select(e => e.Value).Distinct().Single();
		}

		public static FileInfo GetAssembly(this FileInfo file, DirectoryInfo directory)
		{
			//directory = directory.GetDirectories(file.GetFileNameWithoutExtension()).SingleOrDefault();
			return directory == null ? null : directory.GetFiles(String.Format("{0}.dll", file.GetAssembly())).SingleOrDefault();
		}

		public static String GetProduct(this FileInfo file)
		{
			return Config.Products.Intersect(file.GetFileNameWithoutExtension().Split('.')).SingleOrDefault();
		}

		public static String GetRegion(this FileInfo file)
		{
			return Config.Regions.Intersect(file.GetFileNameWithoutExtension().Split('.')).SingleOrDefault();
		}

		public static String GetSolution(this FileInfo file)
		{
			throw new NotImplementedException();
			//return Config.Solutions.OrderByDescending(p => p.Key.Length).First(p => file.Name.StartsWith(String.Format("Wonga.{0}.", p.Key), true, null)).Value;
		}

		public static String GetName(this Type type)
		{
			XmlRootAttribute root = type.GetAttribute<XmlRootAttribute>();
			XmlTypeAttribute xtype = type.GetAttribute<XmlTypeAttribute>();
			return (root == null || String.IsNullOrEmpty(root.ElementName) ? null : root.ElementName) ?? (xtype == null || String.IsNullOrEmpty(xtype.TypeName) ? null : xtype.TypeName) ?? type.Name;
		}

		public static Boolean IsRequest(this Type type)
		{
			return type.GetInterfaces().Any(i => i.FullName == Config.Request);
		}

		public static String GetClean(this Type type)
		{
			return String.Join(null, type.GetName().GetCamel().Select(s => s.ToTitle()).Except(Config.Products, StringComparer.InvariantCultureIgnoreCase).Except(Config.Regions, StringComparer.InvariantCultureIgnoreCase).Except(Config.Suffixes, StringComparer.InvariantCultureIgnoreCase));
		}

		public static String GetSuffix(this Type type)
		{
			if (type.IsRequest())
			{
				if (type.IsCommand())
					return "Command";
				if (type.IsQuery())
					return "Query";
			}
			if (type.IsMessage())
			{
				if (type.IsClass)
					return "Command";
				if (type.IsInterface)
					return "Event";
			}
			throw new NotImplementedException();
		}

		public static Boolean IsCommand(this Type type)
		{
			return type.GetBase().Any(t => t.FullName == Config.Command);
		}

		public static Boolean IsQuery(this Type type)
		{
			return type.GetInterfaces().Any(t => t.FullName == Config.Query);
		}

		public static Boolean IsMessage(this Type type)
		{
			return type.GetInterfaces().Any(t => t.FullName == Config.Message);
		}

		public static IEnumerable<Type> GetBase(this Type type, Boolean self = false)
		{
			if (self)
				yield return type;
			if (type.BaseType != null)
				foreach (Type super in type.BaseType.GetBase(true))
					yield return super;
		}

		public static List<Type> GetTypes(this Type type)
		{
			List<Type> types = new List<Type>();

			Type super = type.BaseType;
			while (super != null && super != typeof(Object))
			{
				if (super.GetInterfaces().Any(i => i.IsMessage()))
					types.Add(super);
				super = super.BaseType;
			}

			types.AddRange(type.GetInterfaces().Where(t => t.GetInterfaces().Any(i => i.FullName == Config.Message)));

			return types;
		}

		public static Dictionary<string, Type> GetMessageMembers(this Type type)
		{
			return type.GetMessageProperties().Select(p => new KeyValuePair<string, Type>(p.Name, p.PropertyType)).Union(type.GetMessageFields().Select(f => new KeyValuePair<string, Type>(f.Name, f.FieldType))).DistinctBy(m => m.Key).ToDictionary(m => m.Key, m => m.Value);
		}

		public static IEnumerable<PropertyInfo> GetMessageProperties(this Type type)
		{
			List<PropertyInfo> properties = type.GetProperties().Where(p => p.CanWrite && !p.IsIgnore()).ToList();
			if (type.IsInterface)
				properties.AddRange(type.GetInterfaces().SelectMany(i => i.GetMessageProperties()));
			return properties;
		}

		public static IEnumerable<FieldInfo> GetMessageFields(this Type type)
		{
			return type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
		}

		public static String ToEnum(this String value)
		{
			return String.Format("{0}Enum", String.Join(null, value.GetCamel().Except(new[] { "Type" })));
		}

		public static String GetNormalTypeName(this String value)
		{
			var s = value.Replace(' ', '_').Where(x => Char.IsLetterOrDigit(x) || x == '_').ToArray();
			return new String(s);
		}

		public static String GetDeclaration(this Type type)
		{
			String name = typeof(Object).Name;
			String ns = type.Namespace.Split('.').First();

			if (ns != "System")
			{
				if (type.IsEnum && ns == "Wonga")
					return type.Name.ToEnum();
				return name;
			}

			if (type.IsGenericType)
			{
				Type definition = type.GetGenericTypeDefinition();
				Type[] arguments = type.GetGenericArguments();

				if (arguments.Length > 1)
					return name;

				String argument = arguments.Single().GetDeclaration();

				if (argument == name)
					return argument;

				if (definition == typeof(Nullable<>))
					return String.Format("{0}?", argument);

				return String.Format("{0}<{1}>", definition.Name.Split('`').First(), argument);
			}

			if (type.IsArray)
				return String.Format("{0}[]", type.GetElementType().GetDeclaration());

			return type.Name;
		}

		public static Boolean IsInstantiatable(this Type type)
		{
			return type.IsInterface || type.IsClass && !type.IsAbstract;
		}

		public static List<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			List<T> list = enumerable.ToList();
			list.ForEach(action);
			return list;
		}

		public static IEnumerable<T> DistinctBy<T, TT>(this IEnumerable<T> enumerable, Func<T, TT> func)
		{
			return enumerable.GroupBy(func).Select(g => g.First());
		}

		public static String[] GetCamel(this String value)
		{
			return Regex.Split(value, "(?<=[^A-Z])(?=[A-Z])|(?<=[^^])(?=[A-Z][^A-Z])");
		}

		public static String ToCamel(this String value)
		{
			return String.Join(null, value.GetCamel().Select(s => s.ToTitle()));
		}

		public static String ToTitle(this String value)
		{
			return value[0].ToString().ToUpper() + value.Substring(1).ToLower();
		}

		public static String Quote(this String value)
		{
			return String.Format("\"{0}\"", value);
		}

		public static StringBuilder AppendFormatLine(this StringBuilder builder, String format, params Object[] args)
		{
			return builder.AppendFormat(format, args).AppendLine();
		}

		public static StringBuilder AppendFormatLine(this StringBuilder builder, IEnumerable<string> format, params Object[] args)
		{
			return builder.AppendFormatLine(String.Join(Environment.NewLine, format), args);
		}

		public static T GetAttribute<T>(this MemberInfo member) where T : Attribute
		{
			return (T)member.GetCustomAttributes(typeof(T), false).SingleOrDefault();
		}

		public static Boolean IsIgnore(this MemberInfo member)
		{
			return member.GetAttribute<XmlIgnoreAttribute>() != null;
		}

		public static String GetName(this MemberInfo member)
		{
			XmlElementAttribute element = member.GetAttribute<XmlElementAttribute>();
			return element == null || String.IsNullOrEmpty(element.ElementName) ? member.Name : element.ElementName;
		}

		public static String GetEnum(this FieldInfo field)
		{
			XmlEnumAttribute attribute = field.GetAttribute<XmlEnumAttribute>();
			return attribute == null || String.IsNullOrEmpty(attribute.Name) ? field.Name : attribute.Name;
		}
	}
}
