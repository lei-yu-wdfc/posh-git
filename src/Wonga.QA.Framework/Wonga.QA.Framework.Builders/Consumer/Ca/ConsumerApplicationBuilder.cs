using System.Collections.Generic;
using Wonga.QA.Framework.Api;

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
			throw new System.NotImplementedException();
		}
	}
}
