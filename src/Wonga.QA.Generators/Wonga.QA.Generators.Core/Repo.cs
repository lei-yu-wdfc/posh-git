using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Wonga.QA.Generators.Core
{
    public static class Repo
    {
        public static String Name { get; set; }
        public static DirectoryInfo Root { get; set; }
        public static DirectoryInfo Src { get; set; }
        public static DirectoryInfo Bin { get; set; }
        public static DirectoryInfo Lib { get; set; }

        static Repo()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            Name = assembly.EntryPoint.DeclaringType.Namespace;
            Root = new FileInfo(assembly.Location).Directory.Parent.Parent;
            Src = Root.GetDirectories("src").Single();
            Bin = Root.GetDirectories("bin").Single();
            Lib = Root.GetDirectories("lib").Single();
        }

        public static FileInfo File(String name, DirectoryInfo directory)
        {
            FileInfo file = new FileInfo(Path.Combine(directory.FullName, name));
            if (file.Exists)
                throw new IOException(file.FullName);
            return file;
        }

        public static DirectoryInfo Directory(String name)
        {
            return Directory(String.Format("{0}.{1}", Name, name), Bin, true);
        }

        public static DirectoryInfo Directory(String name, DirectoryInfo directory, Boolean delete = false)
        {
            directory = new DirectoryInfo(Path.Combine(directory.FullName, name));
            if (delete)
                while (directory.Exists)
                    try { directory.Delete(true); }
                    catch { }
                    finally { directory.Refresh(); }
            if (!directory.Exists)
                directory.Create();
            return directory;
        }

        public static void Inject(DirectoryInfo items, String folder, String project)
        {
            FileInfo file = Src.GetFiles(String.Format("{0}.csproj", project), SearchOption.AllDirectories).Single();
            DirectoryInfo directory = Directory(folder, file.Directory, true);

            Copy(items, directory);

            XElement root = XDocument.Load(file.FullName).Root;
            XName name = root.GetDefaultNamespace().GetName("Compile");

            List<XElement> elements = root.Descendants(name).ToList();
            XElement parent = elements.First().Parent;
            elements.Where(e => e.Attribute("Include").Value.Split('\\').First() == folder).ForEach(e => e.Remove());

            foreach (FileInfo item in directory.GetFiles("*", SearchOption.AllDirectories))
            {
                XElement element = new XElement(name);
                element.SetAttributeValue("Include", item.FullName.Substring(directory.Parent.FullName.Length + 1));
                parent.Add(element);
            }

            root.Document.Save(file.FullName);
        }

        public static void Copy(DirectoryInfo from, DirectoryInfo to)
        {
            from.GetFiles().ForEach(f => f.CopyTo(Path.Combine(to.FullName, f.Name)));
            from.GetDirectories().ForEach(d => Copy(d, to.CreateSubdirectory(d.Name)));
        }
    }
}
