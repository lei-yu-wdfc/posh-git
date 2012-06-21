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
    	public const string MessagesDirectoryName = "Messages";
		public const string EnumsDirectoryName = "Enums";

        public static void Main(String[] args)
        {
            if (args.Any())
                Config.Origin = args.Single();

            var binRootDirectories = new
            {
				Messages = Repo.Directory(MessagesDirectoryName),
				Enums = Repo.Directory(EnumsDirectoryName)
            };

			var assemblies = new List<Assembly>();
        	var enumGenerator = new EnumGenerator();

            foreach (FileInfo file in Origin.GetProjects().OrderBy(f => f.Name))
            {
                Console.WriteLine(file.Name);

                FileInfo assembly1 = file.GetAssembly(Origin.Build);
                if (assembly1 == null)
                    continue;

                Assembly assembly = Assembly.LoadFrom(assembly1.FullName);
                if (assemblies.Contains(assembly))
                    continue;
                assemblies.Add(assembly);

                //TODO
                Boolean cs = file.GetName().Split('.').Contains("Csapi", StringComparer.InvariantCultureIgnoreCase);

                foreach (Type message in assembly.GetTypes().Where(t => t.IsMessage() && t.IsInstantiatable()))
                {
                    String messageClassName = String.Format("{0}{1}{2}{3}{4}{5}", message.GetClean(), GetCollision(message), file.GetProduct(), file.GetRegion(), cs ? "Cs" : null, message.GetSuffix());

					enumGenerator.StartEnumGenerationForClass();

					try
					{
						//now get a folder name to avoid collision
						string messageClassNamespaceRelativePath = message.Namespace != null
												? message.Namespace.Replace("Wonga.", string.Empty)
												: "Default";

						string messageClassSubfolderName = messageClassNamespaceRelativePath.Replace(".", new string(Path.DirectorySeparatorChar, 1));

						String messageClassNamespace = string.Format("{0}.{1}.{2}", Config.Msmq.Project, MessagesDirectoryName, messageClassNamespaceRelativePath);

						DirectoryInfo messageClassDirectory = Repo.Directory(messageClassSubfolderName, binRootDirectories.Messages);
						FileInfo code = Repo.File(String.Format("{0}.cs", messageClassName), messageClassDirectory);

						string generatedEnumNamespace = string.Format("{0}.{1}.{2}", Config.Msmq.Project, EnumsDirectoryName, messageClassNamespaceRelativePath);

						// misses all the include directives
						var builder = InitializeMessageClassDefinition(messageClassName, message, messageClassNamespace);

						foreach (KeyValuePair<String, Type> member in message.GetMessageMembers())
						{
							builder.AppendFormatLine("        public {0} {1} {{ get; set; }}", member.Value.GetDeclaration(), member.Key);

							try
							{
								enumGenerator.GenerateAllEnumsUsedByClassMember(member.Value, generatedEnumNamespace, binRootDirectories.Enums, messageClassSubfolderName);
							}
							catch(Exception e)
							{
								Console.WriteLine("\t*** FAILED ENUM GENERATION FOR CLASS: {0}. {1}", messageClassName, e.Message);
							}							
						}

						builder.AppendLine("    }").AppendLine("}");

						//now insert all the include directives at the begining of the file!!!!
						InsertUsingDirectivesOnMessageClassDefinition(builder, enumGenerator.CurrentClassGeneratedEnumNamespaces);


						using (StreamWriter writer = code.CreateText())
							writer.Write(builder);

						Console.WriteLine("\t{0} \u2192 {1}", message.Name, code.Name);
					}
					catch(Exception e)
					{
						Console.WriteLine("\t*** FAILED GENERATION FOR MESSAGE: {0}. {1}", messageClassName, e.Message);
					}
                }
            }

            Repo.Inject(binRootDirectories.Messages, Config.Msmq.Folder, Config.Msmq.Project);
            Repo.Inject(binRootDirectories.Enums, "Enums", Config.Msmq.Project);
        }

    	private static StringBuilder InitializeMessageClassDefinition(string messageClassName, Type message,
    	                                                              string messageNamespace)
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
    		                                                             messageNamespace,
    		                                                             message.FullName,
    		                                                             message.Name.Quote(),
    		                                                             message.Namespace.Quote(),
    		                                                             String.Join(",", message.GetTypes().Select(t => t.FullName)).Quote(),
    		                                                             messageClassName);
    		return builder;
    	}

    	private static void InsertUsingDirectivesOnMessageClassDefinition(StringBuilder builder, IEnumerable<string> messageGeneratedEnumNamespaces)
    	{
    		StringBuilder usingDirectivesBuilder = new StringBuilder()
    			.AppendLine("using System;")
				.AppendLine("using System.Collections.Generic;")
				.AppendLine("using System.Xml.Serialization;")
				.AppendLine("");

			foreach (string messageGeneratedEnumNamespace in messageGeneratedEnumNamespaces)
    		{
				usingDirectivesBuilder.AppendFormatLine("using {0};", messageGeneratedEnumNamespace);
    		}
    		usingDirectivesBuilder.AppendLine("");

    		builder.Insert(0, usingDirectivesBuilder.ToString());
    	}
    	
        private static String GetCollision(Type type)
        {
            switch (type.FullName)
            {
                case "Wonga.ExperianBulk.InternalMessages.BaseSagaMessage":
                    return "ExperianBulk";
                case "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage":
                    return "Payments";

                case "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent":
                    return "ContactManagement";
                case "Wonga.Comms.PublicMessages.ICommsEvent":
                    return "Comms";

                case "Wonga.Risk.WorkflowDecisions.IWorkflowDecision":
                    return "Internal";
                case "Wonga.Risk.IWorkflowDecision":
                    return "Public";

                case "Wonga.Comms.InternalMessages.FileStorage.SaveFileMessage":
                    return "Comms";
                case "Wonga.FileStorage.PublicMessages.SaveFileMessage":
                    return "FileStorage";

                case "Wonga.Payments.SubmitCounterOffer":
                    return "Payments";
                case "Wonga.Risk.SubmitCounterOfferMessage":
                    return "Risk";

                case "Wonga.CallReport.Batch.Handlers.InternalMessages.UpdateScheduleMessage":
                    return "CallReport";
                case "Wonga.ExperianBulk.InternalMessages.UpdateScheduleMessage":
                    return "ExperianBulk";

                case "Wonga.BankGateway.InternalMessages.Scotiabank.Ca.BasePaymentMessage":
                    return "BankGatewayScotiabank";
                case "Wonga.BankGateway.InternalMessages.Bmo.Ca.BasePaymentMessage":
                    return "BankGatewayBmo";

                case "Wonga.Sms.InternalMessages.SendSmsMessage":
                    return "Sms";
                case "Wonga.Comms.InternalMessages.Za.SendSmsMessage":
                    return "Comms";
                case "Wonga.Comms.InternalMessages.Sms.SendSmsMessage":
                    return "CommsSms";

                default:
                    return null;
            }
        }
    }
}
