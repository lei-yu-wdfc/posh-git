using Wonga.QA.Framework.Core.Exceptions;

namespace Wonga.QA.Framework.Core
{
	public class MessageBase
	{
		public virtual void Default()
		{
			throw new NoDefaultException(this);
		}
	}
}
