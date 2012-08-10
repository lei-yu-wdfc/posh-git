using System;

namespace Wonga.QA.Framework.Application
{
	public abstract class ApplicationBase
	{
		public Guid Id { get; private set; }


		protected ApplicationBase(Guid accountId)
		{
			Id = accountId;
		}
	}
}
