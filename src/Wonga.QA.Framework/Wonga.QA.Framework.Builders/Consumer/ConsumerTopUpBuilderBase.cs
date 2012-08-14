using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
    public abstract class ConsumerTopUpBuilderBase
    {
        protected ConsumerTopUpDataBase TopUpData;

        public ConsumerTopUpBuilderBase(Guid customerId, Guid applicationId, int amount)
        {
            TopUpData.customerId = Convert.ToString(customerId);
            TopUpData.applicationId = Convert.ToString(applicationId);
            TopUpData.amount = amount;
        }

        public TopUp Build()
        {
            RequestTopUp();
            if(TopUpData.HasStatusAccepted)
                AcceptTopUp();
            TopUpData.interestAndFeesAmount = (decimal)GetInterest(TopUpData.amount);
            TopUpData.totalToRepay = (decimal)GetTotalRepayble(TopUpData.amount);
            return CreateTopUp((double)TopUpData.interestAndFeesAmount, (double)TopUpData.totalToRepay, new Guid(TopUpData.FixedTermLoanTopupId), new Guid(TopUpData.customerId), new Guid(TopUpData.applicationId));
        }

        public ConsumerTopUpBuilderBase OnlyInRequest()
        {
            TopUpData.HasStatusAccepted = false;
            return this;
        }

        protected abstract double GetInterest(int amount);
        protected abstract double GetTotalRepayble(int amount);
        protected abstract TopUp CreateTopUp(double interest, double totalToRepay, Guid topUpId, Guid customerId, Guid applicationId);
        protected abstract void RequestTopUp();
        protected abstract void AcceptTopUp();
    }
}
