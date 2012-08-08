using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Builders.Consumer;

namespace Wonga.QA.Framework.Builders.PayLater.Uk
{
	public class PayLaterAccountBuilder : PayLaterAccountBuilderBase
	{
		public PayLaterAccountBuilder(PayLaterAccountDataBase accountData) : base(accountData)
		{
		}

		public PayLaterAccountBuilder(Guid accountId, PayLaterAccountDataBase accountData) : base(accountId, accountData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			throw new NotImplementedException();
		}
	}
}