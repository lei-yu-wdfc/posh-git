using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Account.PayLater;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Api.Requests.Risk.Queries.PayLater.Uk;
using Wonga.QA.Framework.Application;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public abstract class PayLaterApplicationBuilderBase
	{
		protected Guid ApplicationId { get; private set; }
		protected PayLaterApplicationDataBase PayLaterApplicationData { get; private set; }
		protected PayLaterAccount Account { get; private set; }


		protected PayLaterApplicationBuilderBase(PayLaterAccount account, PayLaterApplicationDataBase applicationData)
		{
			ApplicationId = Guid.NewGuid();
			Account = account;
			PayLaterApplicationData = applicationData;
		}

		public PayLaterApplication Build()
		{
			CreateApplication();
			WaitForApplicationDecision();
			WaitForApplicationToBecomeLive();

			return new PayLaterApplication(ApplicationId);
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
            yield return SubmitClientWatermarkCommand.New(r =>
            {
                r.ApplicationId = ApplicationId;
                r.AccountId = Account.Id;
                r.BlackboxData = PayLaterApplicationData.IovationResponse.ToString();
            });
		}

		protected abstract IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		protected abstract void WaitForApplicationToBecomeLive();

		private void WaitForApplicationDecision()
		{
			Do.Until(() => GetApplicationDecision() == PayLaterApplicationData.ExpectedDecision);
		}

		private ApplicationDecisionStatus GetApplicationDecision()
		{
            var decision =
                    Drive.Api.Queries.Post(
                    new GetPaylaterApplicationDecision { ApplicationId = ApplicationId })
                    .Values["ApplicationDecisionStatus"]
                    .Single();

            return (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), decision);
		}

        private void SignApplicationIfRequired()
        {
            if (PayLaterApplicationData.SignIfAccepted)
            {
                if (PayLaterApplicationData.ExpectedDecision == ApplicationDecisionStatus.Accepted ||
                    PayLaterApplicationData.ExpectedDecision == ApplicationDecisionStatus.ReadyToSign)
                {
                    Drive.Api.Commands.Post(new SignApplicationCommand { AccountId = Account.Id, ApplicationId = ApplicationId });
                }
            }
        }

		#region "With" Methods
		#endregion
	}
}
