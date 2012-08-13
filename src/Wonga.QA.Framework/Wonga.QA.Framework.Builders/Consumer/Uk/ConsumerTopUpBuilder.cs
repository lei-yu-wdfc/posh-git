using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;

namespace Wonga.QA.Framework.Builders.Consumer.Uk
{
    class ConsumerTopUpBuilder : ConsumerTopUpBaseBuilder
    {
        public ConsumerTopUpBuilder(Customer customer, Application application, int amount)
            : base(customer, application, amount)
        {
        }

        public TopUp Build()
        {
            TopUpData.interestAndFeesAmount = GetTopUpInterest(TopUpData.amount);
            TopUpData.totalToRepay = GetTopUpRepayble(TopUpData.amount);
            return CreateTopUp(TopUpData.interestAndFeesAmount, TopUpData.totalToRepay, TopUpData.FixedTermLoanTopupId);
        }


        public override double GetTopUpInterest(int amount)
        {
            var responce = Drive.Api.Queries.Post(GetFixedTermLoanTopupCalculationQuery.New(r =>
            {
                r.ApplicationId = TopUpData.application.Id;
                r.TopupAmount = TopUpData.amount;
            }));

            return Convert.ToDouble(responce.Values["InterestAndFeesAmount"].Single());
        }

        public override double GetTopUpRepayble(int amount)
        {
            var responce = Drive.Api.Queries.Post(GetFixedTermLoanTopupCalculationQuery.New(r =>
            {
                r.ApplicationId = TopUpData.application.Id;
                r.TopupAmount = TopUpData.amount;
            }));

            return Convert.ToDouble(responce.Values["TotalRepayable"].Single());
        }

        public override TopUp CreateTopUp(double interest, double totalToRepay, Guid TopUpId)
        {
            return new TopUp(interest, totalToRepay, TopUpId);
        }

        public override void RequestTopUp()
        {
            var responce = Drive.Api.Queries.Post(CreateFixedTermLoanTopupCommand.New(r =>
            {
                r.ApplicationId = TopUpData.application.Id;
                r.TopupAmount = TopUpData.amount;
            }));
            TopUpData.FixedTermLoanTopupId = new Guid(responce.Values["FixedTermLoanTopupId"].Single());
        }

        public override void AcceptTopUp()
        {
            var responce = Drive.Api.Queries.Post(SignFixedTermLoanTopupCommand.New(r =>
                {
                    r.AccountId = TopUpData.customer.Id;
                    r.FixedTermLoanTopupId = TopUpData.FixedTermLoanTopupId;
                }));
        }
    }
}
