using System;

namespace Wonga.QA.Framework.Application
{
	public class ConsumerApplication : ApplicationBase
	{
		public ConsumerApplication(Guid accountId) : base(accountId)
		{
		}

		public override void Repay()
		{
			throw new NotImplementedException();
		}

		public override void Repay(decimal amount)
		{
			throw new NotImplementedException();
		}
	}
}
