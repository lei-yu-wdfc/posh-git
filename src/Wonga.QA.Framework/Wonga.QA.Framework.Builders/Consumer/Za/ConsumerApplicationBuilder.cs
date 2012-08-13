using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Za;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer.Za
{
	public class ConsumerApplicationBuilder : ConsumerApplicationBuilderBase
	{
		public ConsumerApplicationBuilder(ConsumerAccount account, ConsumerApplicationDataBase consumerApplicationData) : base(account, consumerApplicationData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			throw new System.NotImplementedException();
		}

		protected override void WaitForApplicationToBecomeLive()
		{
			Do.Until(() => Drive.Api.Queries.Post(new GetAccountSummaryZaQuery { AccountId = Account.Id }).Values["HasCurrentLoan"].Single() == "true");
		}
	}
}
