using System;

namespace Wonga.QA.Framework.Msmq
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SourceAssemblyAttribute:Attribute
	{
		public string Name { get; private set; }

		public SourceAssemblyAttribute(string name)
		{
			Name = name;
		}
	}
}
