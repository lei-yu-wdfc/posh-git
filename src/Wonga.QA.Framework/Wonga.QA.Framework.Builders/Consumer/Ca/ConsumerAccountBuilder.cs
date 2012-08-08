using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Builders.Consumer.Ca
{
	public class ConsumerAccountBuilder : ConsumerAccountBuilderBase
	{
		public ConsumerAccountBuilder(ConsumerAccountDataBase consumerAccountData) : base(consumerAccountData)
		{
		}

		public ConsumerAccountBuilder(Guid accountId, ConsumerAccountDataBase consumerAccountData) : base(accountId, consumerAccountData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			throw new NotImplementedException();
		}

		protected override void CompletePhoneVerification()
		{
			throw new NotImplementedException();
		}
	}
}
