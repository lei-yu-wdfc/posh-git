using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Queries.PayLater.Uk;
using Wonga.QA.Framework.Application;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater
{
    public abstract class PayLaterApplicationBuilderBase : ApplicationBuilderBase<PayLaterApplication>
	{
		protected Guid ApplicationId { get; private set; }
		protected PayLaterApplicationDataBase PayLaterApplicationData { get; private set; }
		protected Guid PrimaryPhoneVerificationId { get; private set; }

		protected PayLaterApplicationBuilderBase(PayLaterAccount account, PayLaterApplicationDataBase applicationData) : base(account)
		{
			ApplicationId = Guid.NewGuid();
			PayLaterApplicationData = applicationData;
			PrimaryPhoneVerificationId = Guid.NewGuid();
		}

        protected PayLaterApplicationBuilderBase(ConsumerAccount account, PayLaterApplicationDataBase applicationData) : base(account)
        {
            ApplicationId = Guid.NewGuid();
            PayLaterApplicationData = applicationData;
        }

        public override PayLaterApplication Build()
		{
			CreateApplication();
			WaitForApplicationDecision();
			SignApplicationIfRequired();
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
            	r.BlackboxData = IovationResponse;
            });
		}

		protected abstract IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		protected abstract void WaitForApplicationToBecomeLive();

		private void WaitForApplicationDecision()
		{
			Do.Until(() => GetApplicationDecision() == ExpectedDecision);
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
            if (SignIfAccepted)
            {
                if (ExpectedDecision == ApplicationDecisionStatus.Accepted ||
                    ExpectedDecision == ApplicationDecisionStatus.ReadyToSign)
                {
                    Drive.Api.Commands.Post(new SignApplicationPayLaterCommand { AccountId = Account.Id, ApplicationId = ApplicationId });
                }
            }
        }

		#region "With" Methods

		public PayLaterApplicationBuilderBase WithTotalAmount(Decimal amount)
		{
			PayLaterApplicationData.TotalAmount = amount;
			return this;
		}

		public PayLaterApplicationBuilderBase WithMerchantId(Guid merchantId)
		{
			PayLaterApplicationData.MerchantId = merchantId;
			return this;
		}

		public PayLaterApplicationBuilderBase WithMerchantReference(String merchantReference)
		{
			PayLaterApplicationData.MerchantReference = merchantReference;
			return this;
		}

		public PayLaterApplicationBuilderBase WithMerchantOrderId(Guid orderId)
		{
			PayLaterApplicationData.MerchantOrderId = orderId;
			return this;
		}

		#endregion
	}
}
