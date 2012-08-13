using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
    public abstract class ConsumerTopUpBaseBuilder
    {
        protected ConsumerTopUpDataBase TopUpData;

        public ConsumerTopUpBaseBuilder(Guid customerId, Guid applicationId, int amount)
        {
            TopUpData.customerId = customerId;
            TopUpData.applicationId = applicationId;
            TopUpData.amount = amount;
        }

        protected abstract double GetTopUpInterest(int amount);
        protected abstract double GetTopUpRepayble(int amount);
        protected abstract TopUp CreateTopUp(double interest, double totalToRepay, Guid topUpId, Guid customerId, Guid applicationId);
        public abstract void RequestTopUp();
        public abstract void AcceptTopUp();
    }
}
