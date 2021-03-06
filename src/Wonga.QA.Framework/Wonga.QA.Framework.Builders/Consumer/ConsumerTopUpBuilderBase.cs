﻿using System;
using Wonga.QA.Framework.Application.Queries.Consumer;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
    public abstract class ConsumerTopupBuilderBase
    {
		protected ConsumerTopupDataBase TopupData;

		protected Guid TopupId { get; private set; }
		protected Guid ApplicationId { get; private set; }

    	protected ConsumerTopupBuilderBase(Guid applicationId, ConsumerTopupDataBase consumerTopupData)
        {
            ApplicationId = applicationId;
    	    TopupId = Guid.NewGuid();
        	TopupData = consumerTopupData;
        }

        public Topup Build()
        {
            RequestTopUp();

            if(TopupData.StatusAccepted)
                AcceptTopUp();

            return new Topup(TopupId, ApplicationId, TopupData.StatusAccepted);
        }

        public ConsumerTopupBuilderBase WithInRequestStatus()
        {
            TopupData.StatusAccepted = false;
            return this;
        }

        protected abstract Decimal GetInterest(int amount);
        protected abstract Decimal GetTotalRepayable(int amount);
        protected abstract void RequestTopUp();
        protected abstract void AcceptTopUp();
    }
}
