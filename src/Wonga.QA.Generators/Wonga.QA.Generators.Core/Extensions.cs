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
        public static Boolean IsRoot(this DirectoryInfo @this)
        {
            return @this.EnumerateDirectories(".git").Any();
        }

        public static String GetName(this FileInfo @this)
        {
            return Path.GetFileNameWithoutExtension(@this.Name);
        }

        public static FileInfo ChangeExtension(this FileInfo file, String extension)
        {
            return new FileInfo(Path.ChangeExtension(file.FullName, extension));
        }

        public static IEnumerable<string> GetTree(this FileInfo @this)
        {
            yield return @this.Name;
            DirectoryInfo directory = @this.Directory;
            while (directory != null && !directory.IsRoot())
            {
                yield return directory.Name;
                directory = directory.Parent;
            }
        }

        public static Boolean IsTest(this FileInfo @this)
        {
            return @this.GetTree().Any(Config.Test.IsMatch);
        }

        public static XmlSchema GetSchema(this FileInfo @this)
        {
            using (XmlReader reader = XmlReader.Create(@this.FullName))
                return XmlSchema.Read(reader, (s, a) => { throw a.Exception; });
        }

        public static IEnumerable<Type> GetTypes(this FileInfo @this)
        {
            return Assembly.LoadFrom(@this.FullName).GetTypes();
        }

        public static String GetAssembly(this FileInfo @this)
        {
            XElement root = XDocument.Load(@this.FullName).Root;
            return root.Descendants(root.GetDefaultNamespace().GetName("AssemblyName")).Select(e => e.Value).Distinct().Single();
        }

        public static FileInfo GetAssembly(this FileInfo @this, DirectoryInfo directory)
        {
            directory = directory.GetDirectories(@this.GetName()).SingleOrDefault();
            return directory == null ? null : directory.GetFiles(String.Format("{0}.dll", @this.GetAssembly())).SingleOrDefault();
        }

        public static String GetProduct(this FileInfo @this)
        {
            return Config.Products.Intersect(@this.GetName().Split('.')).SingleOrDefault();
        }

        public static String GetRegion(this FileInfo @this)
        {
            return Config.Regions.Intersect(@this.GetName().Split('.')).SingleOrDefault();
        }

        public static DirectoryInfo GetSolution(this FileInfo @this)
        {
            DirectoryInfo directory = @this.Directory;
            while (directory.Parent != null && !directory.IsRoot())
            {
                if (Config.Roots.Contains(directory.Parent.Name))
                    return directory;
                directory = directory.Parent;
            }
            throw new DirectoryNotFoundException();
        }

        public static String GetName(this Type @this)
        {
            XmlRootAttribute root = @this.GetAttribute<XmlRootAttribute>();
            XmlTypeAttribute type = @this.GetAttribute<XmlTypeAttribute>();
            return (root == null || String.IsNullOrEmpty(root.ElementName) ? null : root.ElementName) ?? (type == null || String.IsNullOrEmpty(type.TypeName) ? null : type.TypeName) ?? @this.Name;
        }

        public static Boolean IsRequest(this Type @this)
        {
            return @this.GetInterfaces().Any(i => i.FullName == Config.Request);
        }

        public static String GetClean(this Type @this)
        {
            return String.Join(null, @this.GetName().GetCamel().Select(s => s.ToTitle()).Except(Config.Products).Except(Config.Regions).Except(Config.Suffixes));
        }

        public static String GetSuffix(this Type @this)
        {
            if (@this.IsRequest())
            {
                if (@this.IsCommand())
                    return "Command";
                if (@this.IsQuery())
                    return "Query";
            }
            if (@this.IsMessage())
            {
                if (@this.IsClass)
                    return "Command";
                if (@this.IsInterface)
                    return "Event";
            }
            throw new NotImplementedException();
        }

        public static Boolean IsCommand(this Type @this)
        {
            return @this.GetBase().Any(t => t.FullName == Config.Command);
        }

        public static Boolean IsQuery(this Type @this)
        {
            return @this.GetInterfaces().Any(t => t.FullName == Config.Query);
        }

        public static Boolean IsMessage(this Type @this)
        {
            return @this.GetInterfaces().Any(t => t.FullName == Config.Message);
        }

        public static IEnumerable<Type> GetBase(this Type @this, Boolean self = false)
        {
            if (self)
                yield return @this;
            if (@this.BaseType != null)
                foreach (Type super in @this.BaseType.GetBase(true))
                    yield return super;
        }

        public static List<Type> GetTypes(this Type @this)
        {
            List<Type> types = new List<Type>();

            Type type = @this.BaseType;
            while (type != null && type != typeof(Object))
            {
                if (type.GetInterfaces().Any(i => i.IsMessage()))
                    types.Add(type);
                type = type.BaseType;
            }

            types.AddRange(@this.GetInterfaces().Where(t => t.GetInterfaces().Any(i => i.FullName == Config.Message)));

            return types;
        }

        public static Dictionary<string, Type> GetMessageMembers(this Type @this)
        {
            return @this.GetMessageProperties().Select(p => new KeyValuePair<string, Type>(p.Name, p.PropertyType)).Union(@this.GetMessageFields().Select(f => new KeyValuePair<string, Type>(f.Name, f.FieldType))).DistinctBy(m => m.Key).ToDictionary(m => m.Key, m => m.Value);
        }

        public static IEnumerable<PropertyInfo> GetMessageProperties(this Type @this)
        {
            List<PropertyInfo> properties = @this.GetProperties().Where(p => p.CanWrite && !p.IsIgnore()).ToList();
            if (@this.IsInterface)
                properties.AddRange(@this.GetInterfaces().SelectMany(i => i.GetMessageProperties()));
            return properties;
        }

        public static IEnumerable<FieldInfo> GetMessageFields(this Type @this)
        {
            return @this.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
        }

        public static String ToEnum(this String @this)
        {
            return String.Format("{0}Enum", String.Join(null, @this.GetCamel().Except(new[] { "Type" })));
        }

        public static String GetDeclaration(this Type @this)
        {
            String name = typeof(Object).Name;
            String ns = @this.Namespace.Split('.').First();

            if (ns != "System")
            {
                if (@this.IsEnum && ns == "Wonga")
                    return @this.Name.ToEnum();
                return name;
            }

            if (@this.IsGenericType)
            {
                Type definition = @this.GetGenericTypeDefinition();
                Type[] arguments = @this.GetGenericArguments();

                if (arguments.Length > 1)
                    return name;

                String argument = arguments.Single().GetDeclaration();

                if (argument == name)
                    return argument;

                if (definition == typeof(Nullable<>))
                    return String.Format("{0}?", argument);

                return String.Format("{0}<{1}>", definition.Name.Split('`').First(), argument);
            }

            if (@this.IsArray)
                return String.Format("{0}[]", @this.GetElementType().GetDeclaration());

            return @this.Name;
        }

        public static Boolean IsInstantiatable(this Type @this)
        {
            return @this.IsInterface || @this.IsClass && !@this.IsAbstract;
        }

        public static List<T> ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            List<T> list = @this.ToList();
            list.ForEach(action);
            return list;
        }

        public static IEnumerable<T> DistinctBy<T, TT>(this IEnumerable<T> @this, Func<T, TT> func)
        {
            return @this.GroupBy(func).Select(g => g.First());
        }

        public static String[] GetCamel(this String @this)
        {
            return Regex.Split(@this, "(?<=[^A-Z])(?=[A-Z])|(?<=[^^])(?=[A-Z][^A-Z])");
        }

        public static String ToCamel(this String value)
        {
            return String.Join(null, value.GetCamel().Select(s => s.ToTitle()));
        }

        public static String ToTitle(this String value)
        {
            return value[0].ToString().ToUpper() + value.Substring(1).ToLower();
        }

        public static String Quote(this String @this)
        {
            return String.Format("\"{0}\"", @this);
        }

        public static StringBuilder AppendFormatLine(this StringBuilder @this, String format, params Object[] args)
        {
            return @this.AppendFormat(format, args).AppendLine();
        }

        public static StringBuilder AppendFormatLine(this StringBuilder @this, IEnumerable<string> format, params Object[] args)
        {
            return @this.AppendFormatLine(String.Join(Environment.NewLine, format), args);
        }

        public static T GetAttribute<T>(this MemberInfo @this) where T : Attribute
        {
            return (T)@this.GetCustomAttributes(typeof(T), false).SingleOrDefault();
        }

        public static Boolean IsIgnore(this MemberInfo @this)
        {
            return @this.GetAttribute<XmlIgnoreAttribute>() != null;
        }

        public static String GetName(this MemberInfo @this)
        {
            XmlElementAttribute element = @this.GetAttribute<XmlElementAttribute>();
            return element == null || String.IsNullOrEmpty(element.ElementName) ? @this.Name : element.ElementName;
        }

        public static String GetEnum(this FieldInfo field)
        {
            XmlEnumAttribute attribute = field.GetAttribute<XmlEnumAttribute>();
            return attribute == null || String.IsNullOrEmpty(attribute.Name) ? field.Name : attribute.Name;
        }
    }
}
