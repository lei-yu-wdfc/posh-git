using NServiceBus;
using Wonga.QA.Framework.Core.Exceptions;

namespace Wonga.QA.Framework.Core
{
	public class MessageBase:IMessage
	{
		public virtual void Default()
		{
			throw new NoDefaultException(this);
		}
	}
}
