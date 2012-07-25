using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Msmq
{
    public class MsmqGenerator
    {
        private EnumGenerator _enumGenerator = new EnumGenerator(Config.Msmq);
        private EntityGenerator _entityGenerator;
        private GeneratorRepoDirectories _binRootDirectories;
        private string _messagesDirectoryName = Config.Msmq.Folder;

        public MsmqGenerator()
        {
            _binRootDirectories = new GeneratorRepoDirectories(_messagesDirectoryName, null);
            _entityGenerator = new EntityGenerator(_binRootDirectories.ClassesDirectory, Config.Msmq.Project,
                                                        _messagesDirectoryName);   
        }

    	public void Generate()
    	{
    	    var files = Origin.Build.GetFiles("Wonga.*.dll", SearchOption.AllDirectories).ToList().DistinctBy(x => x.Name);

            foreach (var file in files)
                GenerateTypesForAssembly(file);

            //Copy from tmp to Framework.Msmq both Messages and Enums folders and inject to csproj.
        	Repo.Inject(_binRootDirectories.ClassesDirectory, Config.Msmq.Folder, Config.Msmq.Project);
			Repo.Inject(_binRootDirectories.EnumsDirectory, Config.Enums.Folder, Config.Msmq.Project);
        }

        private void GenerateTypesForAssembly(FileInfo file)
        {
            Assembly assembly = LoadAssembly(file);
            var typesToGenerate = assembly.GetTypes().Where(t => t.IsMessage());
            if(typesToGenerate.Count() == 0)
                return;

            Console.WriteLine(file.FullName);
            foreach (Type message in typesToGenerate)
            {
                String messageClassName = String.Format("{0}", message.GetName());
                Console.Write("\t{0}", message.Name);                
                GeneratedEntityDefinition generatedEntityDefinition = _entityGenerator.GenerateEntityDefinition(message);
                FileInfo code = Repo.File(String.Format("{0}.cs", messageClassName),
                                            generatedEntityDefinition.Directory);
                var builder = InitializeMessageClassDefinition(messageClassName, message, generatedEntityDefinition.Namespace);
                _enumGenerator.StartEnumGenerationForClass(generatedEntityDefinition.Namespace);
                foreach (KeyValuePair<String, Type> member in message.GetMessageMembers())
                {
                    builder.AppendFormatLine("        public {0} {1} {{ get; set; }}",
                                                member.Value.GetDeclaration(), member.Key);

                    _enumGenerator.GenerateAllEnumsUsedByClassMember(member.Value,
                                                                    _binRootDirectories.EnumsDirectory);
                }
                builder.AppendLine("    }").AppendLine("}");
                InsertUsingDirectivesOnMessageClassDefinition(builder, _enumGenerator.GetEnumUsingDirectivesForCurrentClass());
                using (StreamWriter writer = code.CreateText())
                    writer.Write(builder);
                Console.WriteLine(" \u2192 {0}", code.Name);                
            }
        }

        private Assembly LoadAssembly(FileInfo fileInfo)
        {
            try
            {
                var assembly = Assembly.LoadFrom(fileInfo.FullName);
                assembly.GetTypes();
                return assembly;
            }
            catch(ReflectionTypeLoadException ex)
            {
                string loaderEx = "";
                ex.LoaderExceptions.ForEach(x => { loaderEx += x.Message + "\n"; });
                throw new Exception(string.Format("{0} \n {1}", ex.Message, loaderEx));
            }
        }

    	private StringBuilder InitializeMessageClassDefinition(string messageClassName, Type message,
    	                                                              string messageClassNamespace)
    	{
    		StringBuilder builder = new StringBuilder().AppendFormatLine(new[]
    		                                                             	{
    		                                                             		"namespace {0}",
    		                                                             		"{{",
    		                                                             		"    /// <summary> {1} </summary>",
    		                                                             		"    [XmlRoot({2}, Namespace = {3}, DataType = {4})]"
    		                                                             		,
    		                                                             		"    public partial class {5} : MsmqMessage<{5}>",
    		                                                             		"    {{"
    		                                                             	},
    		                                                             messageClassNamespace,
    		                                                             message.FullName,
    		                                                             message.Name.Quote(),
    		                                                             message.Namespace.Quote(),
    		                                                             String.Join(",", message.GetTypes().Select(t => t.FullName)).Quote(),
    		                                                             messageClassName);
    		return builder;
    	}

		private void InsertUsingDirectivesOnMessageClassDefinition(StringBuilder builder, string enumUsingDirectives)
    	{
    		StringBuilder usingDirectivesBuilder = new StringBuilder()
    			.AppendLine("using System;")
    			.AppendLine("using System.Collections.Generic;")
    			.AppendLine("using System.Xml.Serialization;")
    			.AppendLine("")
    			.Append(enumUsingDirectives);

			builder.Insert(0, usingDirectivesBuilder.ToString());
    	}
    }
}
