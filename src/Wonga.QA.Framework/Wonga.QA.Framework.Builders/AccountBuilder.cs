using System;
using Wonga.QA.Framework.Builders.Consumer;
using Wonga.QA.Framework.Builders.PayLater;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders
{
	public class AccountBuilder
	{
		public static class Consumer
		{
			public static ConsumerAccountBuilderBase New()
			{
				var accountId = Guid.NewGuid();
				var accountData = new ConsumerAccountDataBase();
				return New(accountId, accountData);
			}

			public static ConsumerAccountBuilderBase New(Guid accountId, ConsumerAccountDataBase accountData)
			{
				switch (Config.AUT)
				{
					case AUT.Ca:
						return new Builders.Consumer.Ca.ConsumerAccountBuilder(accountId, accountData);
					case AUT.Uk:
						return new Builders.Consumer.Uk.ConsumerAccountBuilder(accountId, accountData);
					case AUT.Za:
						return new Builders.Consumer.Za.ConsumerAccountBuilder(accountId, accountData);
				}

				throw new NotSupportedException(Config.AUT.ToString());
			}
		}

		public static class PayLater
		{
			public static PayLaterAccountBuilderBase New()
			{
				var accountId = Guid.NewGuid();
				var accountData = new PayLaterAccountDataBase();

				return New(accountId, accountData);
			}

			public static PayLaterAccountBuilderBase New(Guid accountId, PayLaterAccountDataBase accountData)
			{
				switch (Config.AUT)
				{
					case AUT.Uk:
						return new Builders.PayLater.Uk.PayLaterAccountBuilder(accountId, accountData);
				}

				throw new NotSupportedException(Config.AUT.ToString());
			}
		}
	}
}
