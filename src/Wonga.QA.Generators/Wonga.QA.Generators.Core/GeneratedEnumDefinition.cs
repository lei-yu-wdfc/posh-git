using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Generators.Core
{
	public class GeneratedEnumDefinition
	{
		public string OriginalName { get; private set; }

		public string GeneratedName { get; private set; }

		public string OriginalNamespace { get; private set; }

		public string OriginalFullName { get; private set; }

		public string GeneratedNamespace { get; set; }

		public IEnumerable<string> ValueNames { get; private set; }

		
		public GeneratedEnumDefinition(Type enumType, string generatedNamespace)
		{
			OriginalName = enumType.Name;
			GeneratedName = enumType.GetName().ToEnum().ToCamel();
			OriginalNamespace = enumType.Namespace;
			OriginalFullName = enumType.FullName;
			GeneratedNamespace = generatedNamespace;
			ValueNames = Enum.GetNames(enumType);
		}
		

		public string GeneratedFullName
		{
			get { return string.Format("{0}.{1}", GeneratedNamespace, GeneratedName); }
		}

	}
}
