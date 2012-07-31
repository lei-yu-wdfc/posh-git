using System;

namespace Wonga.QA.Framework.Msmq
{
	public class MissingSourceAssemblyAttributeException:Exception
	{
		public MissingSourceAssemblyAttributeException(Type type):base(type.FullName){}
	}
}
