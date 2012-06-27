
using System;

namespace Wonga.QA.Framework.Core.Exceptions
{
	public class NoDefaultException : NotImplementedException
	{
		public NoDefaultException(MessageBase message)
			: base(String.Format("Override Default in {0}.", message.GetType().Name))
		{
		}
	}
}
