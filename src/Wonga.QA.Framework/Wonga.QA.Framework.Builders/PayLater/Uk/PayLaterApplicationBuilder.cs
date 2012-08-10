using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Account.PayLater;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Builders.Consumer;

namespace Wonga.QA.Framework.Builders.PayLater.Uk
{
	public class PayLaterApplicationBuilder : PayLaterApplicationBuilderBase
	{
		public PayLaterApplicationBuilder(PayLaterAccount account, PayLaterApplicationDataBase applicationData) : base(account, applicationData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			throw new NotImplementedException();
		}

		protected override void WaitForApplicationToBecomeLive()
		{
			throw new NotImplementedException();
		}
	}
}
