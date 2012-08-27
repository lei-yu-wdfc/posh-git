using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Application;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;

namespace Wonga.QA.Framework.Builders.Consumer
{
    public abstract class ConsumerApplicationBuilderBase : ApplicationBuilderBase<ConsumerApplication>
	{
		protected Guid ApplicationId { get; private set; }
		protected ConsumerApplicationDataBase ConsumerApplicationData { get; private set; }
		protected ConsumerAccount Account { get; private set; }


		protected ConsumerApplicationBuilderBase(ConsumerAccount account, ConsumerApplicationDataBase consumerApplicationData) : base(account)
		{
			ApplicationId = Guid.NewGuid();
			Account = account;
			ConsumerApplicationData = consumerApplicationData;
		}

		public override ConsumerApplication Build()
		{
			CreateApplication();
			WaitForApplicationDecision();
			SignApplicationIfRequired();
			WaitForApplicationToBecomeLive();

			return new ConsumerApplication(ApplicationId);
		}

		private void CreateApplication()
		{
			var commands = new List<ApiRequest>();
			commands.AddRange(GetGenericApiCommands());
			commands.AddRange(GetRegionSpecificApiCommands());
			Drive.Api.Commands.Post(commands);
		}

		private IEnumerable<ApiRequest> GetGenericApiCommands()
		{
			yield return SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = ApplicationId);

			yield return SubmitClientWatermarkCommand.New(r =>
			                                              	{
			                                              		r.ApplicationId = ApplicationId;
			                                              		r.AccountId = Account.Id;
			                                              		r.BlackboxData = IovationResponse.ToString();
			                                              	});
		}

		protected abstract IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		protected abstract void WaitForApplicationToBecomeLive();

		private void WaitForApplicationDecision()
		{
			if (ExpectedDecision == null)
				return;

			Do.Until(() => GetApplicationDecision() == ExpectedDecision);
		}

		private ApplicationDecisionStatus GetApplicationDecision()
		{
			var decision =
				Drive.Api.Queries.Post(
				new GetApplicationDecisionQuery {ApplicationId = ApplicationId})
				.Values["ApplicationDecisionStatus"]
				.Single();

			return (ApplicationDecisionStatus) Enum.Parse(typeof (ApplicationDecisionStatus), decision);
		}

		private void SignApplicationIfRequired()
		{
			if(SignIfAccepted)
			{
				if (ExpectedDecision == ApplicationDecisionStatus.Accepted || 
					ExpectedDecision == ApplicationDecisionStatus.ReadyToSign)
				{
					Drive.Api.Commands.Post(new SignApplicationCommand { AccountId = Account.Id, ApplicationId = ApplicationId });
				}
			}
		}

		#region "With" Methods

		public ConsumerApplicationBuilderBase WithLoanAmount(Decimal amount)
		{
			ConsumerApplicationData.LoanAmount = amount;
			return this;
		}

		public ConsumerApplicationBuilderBase WithPromiseDate(Date date)
		{
			ConsumerApplicationData.PromiseDate = date;
			return this;
		}

		#endregion
	}
}
