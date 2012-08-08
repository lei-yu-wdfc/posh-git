using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer.Ca
{
	public class ConsumerApplicationBuilder : ConsumerApplicationBuilderBase
	{
		public ConsumerApplicationBuilder(Customer consumerAccountBase, ConsumerApplicationDataBase consumerApplicationData) : base(consumerAccountBase, consumerApplicationData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			throw new System.NotImplementedException();
		}

		protected override void WaitForApplicationToBecomeLive()
		{
			Do.Until(() => Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = ConsumerAccountBase.Id }).Values["HasCurrentLoan"].Single() == "true");
		}
	}
}
