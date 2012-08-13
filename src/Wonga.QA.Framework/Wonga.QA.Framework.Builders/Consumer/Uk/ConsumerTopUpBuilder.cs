using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var responce = Drive.Api.Queries.Post(GetFixedTermLoanTopupCalculationQuery.New(r =>
            {
                r.ApplicationId = _application.Id;
                r.TopupAmount = _amount;
            }));

            _interestAndFeesAmount = Convert.ToDouble(responce.Values["InterestAndFeesAmount"].Single());
            _totalToRepay = Convert.ToDouble(responce.Values["TotalRepayable"].Single());

            return new TopUp(_interestAndFeesAmount, _totalToRepay);
        }
    }
}
