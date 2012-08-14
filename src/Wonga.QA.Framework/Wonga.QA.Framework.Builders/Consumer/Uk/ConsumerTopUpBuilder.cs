using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;

namespace Wonga.QA.Framework.Builders.Consumer.Uk
{
    class ConsumerTopUpBuilder : ConsumerTopUpBuilderBase
    {
        public ConsumerTopUpBuilder(Guid customerId, Guid applicationId, int amount)
            : base(customerId, applicationId, amount)
        {
        }

        protected override double GetInterest(int amount)
        {
            var response = Drive.Api.Queries.Post(GetFixedTermLoanTopupCalculationQuery.New(r =>
            {
                r.ApplicationId = TopUpData.applicationId;
                r.TopupAmount = TopUpData.amount;
            }));

            return Convert.ToDouble(response.Values["InterestAndFeesAmount"].Single());
        }

        protected override double GetTotalRepayble(int amount)
        {
            var response = Drive.Api.Queries.Post(GetFixedTermLoanTopupCalculationQuery.New(r =>
            {
                r.ApplicationId = TopUpData.applicationId;
                r.TopupAmount = TopUpData.amount;
            }));

            return Convert.ToDouble(response.Values["TotalRepayable"].Single());
        }

        protected override TopUp CreateTopUp(double interest, double totalToRepay, Guid topUpId, Guid customerId, Guid applicationId)
        {
            return new TopUp(interest, totalToRepay, topUpId, customerId, applicationId);
        }

        public override void RequestTopUp()
        {
            var response = Drive.Api.Queries.Post(CreateFixedTermLoanTopupCommand.New(r =>
            {
                r.ApplicationId = TopUpData.applicationId;
                r.TopupAmount = TopUpData.amount;
            }));
            TopUpData.FixedTermLoanTopupId = response.Values["FixedTermLoanTopupId"].Single();
        }

        public override void AcceptTopUp()
        {
            var response = Drive.Api.Queries.Post(SignFixedTermLoanTopupCommand.New(r =>
                {
                    r.AccountId = TopUpData.customerId;
                    r.FixedTermLoanTopupId = TopUpData.FixedTermLoanTopupId;
                }));
        }
    }
}
