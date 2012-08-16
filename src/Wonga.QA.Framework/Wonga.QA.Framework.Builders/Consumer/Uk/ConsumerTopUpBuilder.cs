using System;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Application.Queries;

namespace Wonga.QA.Framework.Builders.Consumer.Uk
{
    class ConsumerTopupBuilder : ConsumerTopupBuilderBase
    {
    	private ApiResponse _cachedTopupCalculationResponse = null;

        public ConsumerTopupBuilder(Guid applicationId, ConsumerTopupDataBase consumerTopupData)
            : base(applicationId, consumerTopupData)
        {
        }

        protected override Decimal GetInterest(int amount)
        {
        	var response = GetTopUpCalculationQueryResponse();
            return Convert.ToDecimal(response.Values["InterestAndFeesAmount"].Single());
        }

        protected override Decimal GetTotalRepayable(int amount)
        {
        	var response = GetTopUpCalculationQueryResponse();
            return Convert.ToDecimal(response.Values["TotalRepayable"].Single());
        }

		private ApiResponse GetTopUpCalculationQueryResponse()
		{
			if(_cachedTopupCalculationResponse == null)
			{
				_cachedTopupCalculationResponse = Drive.Api.Queries.Post(GetFixedTermLoanTopupCalculationQuery.New(r =>
				{
					r.ApplicationId = ApplicationId;
					r.TopupAmount = TopupData.Amount;
				}));
			}

			return _cachedTopupCalculationResponse;
		}

        protected override void RequestTopUp()
        {
            var response = Drive.Api.Queries.Post(CreateFixedTermLoanTopupCommand.New(r =>
			{
				r.ApplicationId = ApplicationId;
				r.TopupAmount = TopupData.Amount;
                r.FixedTermLoanTopupId = TopupId;
			    r.AccountId = ApplicationQueries.Consumer.GetAccountGuidForApplication(ApplicationId);
			}));
        }

        protected override void AcceptTopUp()
        {
			Drive.Api.Queries.Post(SignFixedTermLoanTopupCommand.New(r =>
			{
				r.AccountId = ApplicationQueries.Consumer.GetAccountGuidForApplication(ApplicationId);
				r.FixedTermLoanTopupId = TopupId;
			}));
        }
    }
}
