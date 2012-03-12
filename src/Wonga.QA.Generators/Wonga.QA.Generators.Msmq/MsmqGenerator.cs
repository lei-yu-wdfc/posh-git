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
        public static void Main(String[] args)
        {
            if (args.Any())
                Config.Origin = args.Single();

            var bin = new
            {
                Messages = Repo.Directory("Messages"),
                Enums = Repo.Directory("Enums")
            };

            Dictionary<String, String[]> enums = new Dictionary<String, String[]>();
            List<Assembly> assemblies = new List<Assembly>();

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
                    String name = String.Format("{0}{1}{2}{3}{4}{5}", message.GetClean(), GetCollision(message), file.GetProduct(), file.GetRegion(), cs ? "Cs" : null, message.GetSuffix());
                    //String root = file.GetSolution();
                    FileInfo code = Repo.File(String.Format("{0}.cs", name), bin.Messages);

                    StringBuilder builder = new StringBuilder().AppendFormatLine(new[]{
                        "using System;",
                        "using System.Collections.Generic;",
                        "using System.Xml.Serialization;",
                        "",
                        "namespace {0}",
                        "{{",
                        "    /// <summary> {1} </summary>",
                        "    [XmlRoot({2}, Namespace = {3}, DataType = {4})]",
                        "    public partial class {5} : MsmqMessage<{5}>",
                        "    {{"
                    }, Config.Msmq.Project, message.FullName, message.Name.Quote(), message.Namespace.Quote(), String.Join(",", message.GetTypes().Select(t => t.FullName)).Quote(), name);

                    foreach (KeyValuePair<String, Type> member in message.GetMessageMembers())
                    {
                        builder.AppendFormatLine("        public {0} {1} {{ get; set; }}", member.Value.GetDeclaration(), member.Key);

                        List<Type> enums2 = new List<Type>();
                        if (member.Value.IsEnum)
                            enums2.Add(member.Value);
                        if (member.Value.IsGenericType)
                            enums2.AddRange(member.Value.GetGenericArguments().Where(a => a.IsEnum));
                        enums2 = enums2.Where(t => t.FullName.StartsWith("Wonga")).ToList();

                        foreach (Type enu in enums2)
                        {
                            String name1 = enu.GetName().ToEnum().ToCamel();
                            String[] values = Enum.GetNames(enu);

                            if (enums.ContainsKey(name1))
                                if (enums[name1].SequenceEqual(values))
                                    continue;
                                else
                                    throw new NotImplementedException();
                            enums.Add(name1, values);

                            FileInfo fenum = Repo.File(String.Format("{0}.cs", name1), bin.Enums);

                            StringBuilder builder1 = new StringBuilder().AppendFormatLine(new[]
	                        {
		                        "namespace {0}",
		                        "{{",
		                        "    public enum {1}",
		                        "    {{"
	                        }, Config.Msmq.Project, name1);

                            foreach (Object value in Enum.GetValues(enu))
                                builder1.AppendFormatLine("        {0} = {1},", value, (int)value);

                            builder1.AppendLine("    }").AppendLine("}");

                            using (StreamWriter writer = fenum.CreateText())
                                writer.Write(builder1);

                            Console.WriteLine("\t{0} \u2192 {1}", enu.Name, fenum.Name);
                        }
                    }

                    builder.AppendLine("    }").AppendLine("}");

                    using (StreamWriter writer = code.CreateText())
                        writer.Write(builder);

                    Console.WriteLine("\t{0} \u2192 {1}", message.Name, code.Name);
                }
            }

            Repo.Inject(bin.Messages, Config.Msmq.Folder, Config.Msmq.Project);
            Repo.Inject(bin.Enums, "Enums", Config.Msmq.Project);
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

                default:
                    return null;
            }
        }
    }
}
