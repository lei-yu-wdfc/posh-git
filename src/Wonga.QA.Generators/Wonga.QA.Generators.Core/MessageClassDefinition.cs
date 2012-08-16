using System;
using System.Linq;

namespace Wonga.QA.Generators.Core
{
	public class MessageClassDefinition
	{
		public String Namespace { get; private set; }
		public String ClassName { get; private set; }
		public String ClassBody { get; private set; }
		public String FileName { get; private set; }
		public String Region { get; private set; }
		public String MessageType { get; private set; }
		public String Component { get; private set; }

		private static readonly String[] Regions = new[] { "Ca", "Pl", "Uk", "Za" };
		private static readonly String[] MessageTypes = new[] {"Command", "Query"};

		public MessageClassDefinition(String @namespace, String className, String classBody)
		{
			Namespace = @namespace;
			ClassName = className;
			ClassBody = classBody;
			FileName = String.Format("{0}.cs", ClassName);
			MessageType = GetMessageType();
			Component = GetComponent();
			Region = GetRegion();
		}

		private String GetRegion()
		{
			foreach (var region in Regions)
			{
				if (MessageTypes.Any(type => ClassName.Contains(region + type)))
				{
					return region;
				}
			}
			return String.Empty;
		}

		private String GetMessageType()
		{
			foreach (var messageType in MessageTypes)
			{
				if (ClassName.Contains(messageType))
				{
					return messageType;
				}
			}

			throw new Exception("Message must be of a defined type");
		}

		private String GetComponent()
		{
			var parts = Namespace.Split('.').ToArray();
			var component = String.Empty;

			var i0 = Array.IndexOf(parts, "Wonga") + 1;
			int i1;
			
			if( MessageType == "Command")
				i1 = Array.IndexOf(parts, "Commands") - 1;

			else
				i1 = Array.IndexOf(parts, "Queries") - 1;

			for(int i = i0; i <= i1 ; i++)
			{
				component += parts[i];
				component += ".";
			}

			return component.TrimEnd('.');
		}
	}
}
