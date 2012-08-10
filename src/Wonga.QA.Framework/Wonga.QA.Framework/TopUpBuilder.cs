using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
//using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Helpers;

namespace Wonga.QA.Framework.Builders
{
    public class TopUpBuilder
    {
        private Customer _customer;
        private Application _application;
        private int _amount;

        public TopUp Build()
        {
            var requests = new List<ApiRequest>
                               {
                                   GetFixedTermLoanTopupOfferQuery.New(r => r.AccountId = Guid.NewGuid())
                               };
            switch (Config.AUT)
            {
                case AUT.Uk:

                    var responce = Drive.Api.Commands.Post(requests);
                    break;
            }

            return new TopUp();
        }

        public static TopUpBuilder New()
        {
            return new TopUpBuilder();
        }

        public TopUpBuilder WithCustomer(Customer _lcustomer)
        {
            _customer = _lcustomer;
            return this;
        }

        public TopUpBuilder WithApplication(Application _lapplication)
        {
            _application = _lapplication;
            return this;
        }

        public TopUpBuilder WithAmount(int _lamount)
        {
            _amount = _lamount;
            return this;
        }

        public TopUpBuilder Request()
        {
            return this;
        }

        public TopUpBuilder Accept()
        {
            return this;
        }

        public TopUpBuilder Decline()
        {
            return this;
        }
    }
}
