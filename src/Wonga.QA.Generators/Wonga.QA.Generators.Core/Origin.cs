using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Wonga.QA.Generators.Core
{
    public static class Origin
    {
        public static DirectoryInfo Root { get; set; }
        public static DirectoryInfo Src { get; set; }
        public static DirectoryInfo Build { get; set; }

        static Origin()
        {
            Root = new DirectoryInfo(Path.Combine(Repo.Root.FullName, Config.Origin));
            Src = Root.GetDirectories("src").Single();
            Build = Root.GetDirectories("build").Single();
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            return GetProjects().Select(f => f.GetAssembly(Build)).Where(f => f != null).Select(f => Assembly.LoadFrom(f.FullName)).Distinct();
        }

        public static IEnumerable<FileInfo> GetProjects()
        {
            return Src.GetFiles("Wonga.*.csproj", SearchOption.AllDirectories).Where(f => !f.IsTest());
        }

        public static IEnumerable<Type> GetTypes()
        {
            return GetAssemblies().SelectMany(a => a.GetTypes()).Distinct();
        }

        public static IEnumerable<FileInfo> GetSchemas()
        {
            return Src.GetFiles("Wonga.*.xsd", SearchOption.AllDirectories).Where(f => !f.IsTest() && !f.IsArtifact());
        }
    }
}
