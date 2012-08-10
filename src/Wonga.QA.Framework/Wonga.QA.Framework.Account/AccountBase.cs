using System;

namespace Wonga.QA.Framework.Account
{
	public abstract class AccountBase
	{
		public Guid Id { get; private set; }

		protected AccountBase(Guid accountId)
		{
			Id = accountId;
		}
	}
}
