using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Msmq
{
    /// <summary>
    /// Let me explain to you how the generator works before you start pulling your hair and jump through a window..
    /// * The generator will process 1 or multiple Repos from Config.Repos list(Payments, Risk, Ops etc)
    /// * For each repo it will try to find the csproj files under that repo, ie v3split\Payments
    /// * For each csproj found it will try to load it's corresponding dll from the v3split\package folder, if for whatever reason this
    ///   fails it will go to the next one without crashing.
    /// * For each assembly loaded it will generate types for the messages and enums and store them under Bin\.Msmq and Bin\.Enums folder.
    /// * Once everything has been processed and given no error occured, it will first of all delete the Messages and Enums folder of the
    ///   Framework.Msmq project.
    /// * It will the temporary directories to the corresponding Messages and Enums folder we deleted in the previous step.
    /// * It will register any files found underneath those folders in the Framework.Msmq.csproj
    /// </summary>
    public class MsmqGenerator
    {
    	public static void Main(String[] args)
    	{
			ProgramArgumentsParser.ParseArgumentsParameters(args);

            string messagesDirectoryName = Config.Msmq.Folder;
            var binRootDirectories = new GeneratorRepoDirectories(messagesDirectoryName, null);
            binRootDirectories.ClassesDirectory.GetDirectories().ForEach(x => x.Delete(true));
            binRootDirectories.EnumsDirectory.GetDirectories().ForEach(x => x.Delete(true));

            foreach (var repo in Config.Repos)
            {
                Config.RepoName = repo;
                bool errorsOccurred = false;
                var enumGenerator = new EnumGenerator(Config.Msmq);
                var entityGenerator = new EntityGenerator(binRootDirectories.ClassesDirectory, Config.Msmq.Project,
                                                          messagesDirectoryName);
                var csprojs = Origin.GetProjects().DistinctBy(x => x.Name).OrderBy(f => f.Name);
                foreach (FileInfo file in csprojs)
                {
                    Console.WriteLine(file.Name);
                    //At this point we try to load a located dll from a folder that has all of its dependencies.
                    var paths = AssemblyPathFinder.GetFiles(file, Origin.Build);
                    Assembly assembly = null;
                    assembly = GetFirstAssemblyFromPaths(paths);
                    //If we couldn't load it then we continue to the next file in the list
                    if (assembly == null)
                        continue;

                    //This foreach loop generates and writes to the tmp folder all the types within the assembly.
                    foreach (Type message in assembly.GetTypes().Where(t => t.IsMessage() && t.IsInstantiatable()))
                    {
                        String messageClassName = String.Format("{0}", message.GetName());

                        try
                        {
                            GeneratedEntityDefinition generatedEntityDefinition =
                                entityGenerator.GenerateEntityDefinition(message);

                            FileInfo code = Repo.File(String.Format("{0}.cs", messageClassName),
                                                      generatedEntityDefinition.Directory);

                            // no include directives (added at the end)
                            var builder = InitializeMessageClassDefinition(messageClassName, message,
                                                                           generatedEntityDefinition.Namespace);

                            enumGenerator.StartEnumGenerationForClass(generatedEntityDefinition.Namespace);

                            foreach (KeyValuePair<String, Type> member in message.GetMessageMembers())
                            {
                                builder.AppendFormatLine("        public {0} {1} {{ get; set; }}",
                                                         member.Value.GetDeclaration(), member.Key);

                                enumGenerator.GenerateAllEnumsUsedByClassMember(member.Value,
                                                                                binRootDirectories.EnumsDirectory);
                            }

                            builder.AppendLine("    }").AppendLine("}");

                            //now insert all the include directives at the begining of the file!!!!
                            InsertUsingDirectivesOnMessageClassDefinition(builder,
                                                                          enumGenerator.
                                                                              GetEnumUsingDirectivesForCurrentClass());

                            using (StreamWriter writer = code.CreateText())
                                writer.Write(builder);

                            Console.WriteLine("\t{0} \u2192 {1}", message.Name, code.Name);
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine("\t*** FAILED GENERATION FOR MESSAGE: {0}. {1}", messageClassName,
                                                    e.Message);
                            errorsOccurred = true;
                        }
                    }
                }

                if (errorsOccurred || enumGenerator.ErrorsOccurred)
                {
                    Console.Error.WriteLine("*** THERE WERE ERRORS DURING GENERATION... NOT UPDATING QAF!!!!!");
                    return;
                }
            }

            //Delete Framework.Msmq Messages and Enums folder 
    	    Directory.GetDirectories(Path.Combine(Repo.Src.GetFiles(Config.Msmq.Project + ".csproj", SearchOption.AllDirectories).Single().Directory.FullName,
    	                 Config.Msmq.Folder)).ForEach(x => Directory.Delete(x , true));
            Directory.GetDirectories(Path.Combine(Repo.Src.GetFiles(Config.Msmq.Project + ".csproj", SearchOption.AllDirectories).Single().Directory.FullName,
                         Config.Enums.Folder)).ForEach(x => Directory.Delete(x, true));

            //Copy from tmp to Framework.Msmq both Messages and Enums folders and inject to csproj.
        	Repo.Inject(binRootDirectories.ClassesDirectory, Config.Msmq.Folder, Config.Msmq.Project);
			Repo.Inject(binRootDirectories.EnumsDirectory, Config.Enums.Folder, Config.Msmq.Project);
        }

        private static Assembly GetFirstAssemblyFromPaths(FileInfo[] paths)
        {
            foreach(var path in paths)
                try
                {
                    Assembly assembly = null;
                    //Some times GetTypes fails eventhough LoadFrom succeeds, thus call it first before assigning.
                    Assembly.LoadFrom(path.FullName).GetTypes();
                    assembly = Assembly.LoadFrom(path.FullName);
                    return assembly;
                }
                catch
                {
                    return null;
                }
            return null;
        }

    	private static StringBuilder InitializeMessageClassDefinition(string messageClassName, Type message,
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

		private static void InsertUsingDirectivesOnMessageClassDefinition(StringBuilder builder, string enumUsingDirectives)
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
