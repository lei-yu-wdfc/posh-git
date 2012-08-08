using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;

namespace Wonga.QA.Framework.Builders.Consumer
{
	public abstract class ConsumerApplicationBuilderBase
	{
		protected Guid ApplicationId { get; private set; }
		protected ConsumerApplicationDataBase ConsumerApplicationData { get; private set; }
		protected Customer ConsumerAccountBase { get; private set; }


		protected ConsumerApplicationBuilderBase(Customer consumerAccountBase, ConsumerApplicationDataBase consumerApplicationData)
		{
			ApplicationId = Guid.NewGuid();
			ConsumerAccountBase = consumerAccountBase;
			ConsumerApplicationData = consumerApplicationData;
		}

		public Application Build()
		{
			CreateApplication();
			WaitForApplicationDecision();
			SignApplicationIfRequired();
			WaitForApplicationToBecomeLive();

			return new Application(ApplicationId);
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
			                                              		r.AccountId = ConsumerAccountBase.Id;
			                                              		r.BlackboxData = ConsumerApplicationData.IovationResponse.ToString();
			                                              	});
		}

		protected abstract IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		protected abstract void WaitForApplicationToBecomeLive();

		private void WaitForApplicationDecision()
		{
			if (ConsumerApplicationData.ExpectedDecision == null)
				return;

			Do.Until(() => GetApplicationDecision() == ConsumerApplicationData.ExpectedDecision);
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
			if( ConsumerApplicationData.SignIfAccepted)
			{
				if (ConsumerApplicationData.ExpectedDecision == ApplicationDecisionStatus.Accepted || 
					ConsumerApplicationData.ExpectedDecision == ApplicationDecisionStatus.ReadyToSign)
				{
					Drive.Api.Commands.Post(new SignApplicationCommand { AccountId = ConsumerAccountBase.Id, ApplicationId = ApplicationId });
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

		public ConsumerApplicationBuilderBase WithExpectedDecision(ApplicationDecisionStatus decision)
		{
			ConsumerApplicationData.ExpectedDecision = decision;
			return this;
		}

		public ConsumerApplicationBuilderBase WithNoExpectedDecision()
		{
			ConsumerApplicationData.ExpectedDecision = null;
			return this;
		}

		public ConsumerApplicationBuilderBase WithIovationResponse(IovationMockResponse iovationResponse)
		{
			ConsumerApplicationData.IovationResponse = iovationResponse;
			return this;
		}

		#endregion

		public ConsumerApplicationBuilderBase WhichIsUnsigned()
		{
			ConsumerApplicationData.SignIfAccepted = false;
			return this;
		}
	}
}
