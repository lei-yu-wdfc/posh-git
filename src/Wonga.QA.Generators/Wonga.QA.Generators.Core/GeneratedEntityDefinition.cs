using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wonga.QA.Generators.Core
{
	public class GeneratedEntityDefinition
	{
		public string Namespace { get; private set; }

		public DirectoryInfo Directory { get; private set; }

		public GeneratedEntityDefinition(string ns, DirectoryInfo directory)
		{
			Namespace = ns;
			Directory = directory;
		}
	}
}
