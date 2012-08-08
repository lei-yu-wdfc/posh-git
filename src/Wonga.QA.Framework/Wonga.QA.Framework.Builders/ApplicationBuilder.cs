using System;
using Wonga.QA.Framework.Builders.Consumer;
using Wonga.QA.Framework.Builders.PayLater;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders
{
	public class ApplicationBuilder
	{
		public static class Consumer
		{
			public static ConsumerApplicationBuilderBase New(Customer account)
			{
				var applicationData = new ConsumerApplicationDataBase();
				return New(account, applicationData);
			}

			public static ConsumerApplicationBuilderBase New(Customer account, ConsumerApplicationDataBase consumerApplicationData)
			{
				switch (Config.AUT)
				{
					case AUT.Ca:
						return new Builders.Consumer.Ca.ConsumerApplicationBuilder(account, consumerApplicationData);
					case AUT.Uk:
						return new Builders.Consumer.Uk.ConsumerApplicationBuilder(account, consumerApplicationData);
					case AUT.Za:
						return new Builders.Consumer.Za.ConsumerApplicationBuilder(account, consumerApplicationData);
				}

				throw new NotSupportedException(Config.AUT.ToString());
			}
		}

		public static class PayLater
		{
			public static PayLaterApplicationBuilderBase New(Customer account)
			{
				var applicationData = new PayLaterApplicationDataBase();
				return New(account, applicationData);
			}

			public static PayLaterApplicationBuilderBase New(Customer account, PayLaterApplicationDataBase consumerApplicationData)
			{
				switch (Config.AUT)
				{
					case AUT.Uk:
						return new Builders.PayLater.Uk.PayLaterApplicationBuilder(account, consumerApplicationData);
				}

				throw new NotSupportedException(Config.AUT.ToString());
			}
		}
	}
}
