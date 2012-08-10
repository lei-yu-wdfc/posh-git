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
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Helpers;

namespace Wonga.QA.Framework.Builders
{
    public class TopUpBuilder
    {
        private Customer _customer;
        private Application _application;
        private int _amount;
        private double _interestAndFeesAmount;
        private double _totalToRepay;
        
        public TopUp Build()
        {
            switch (Config.AUT)
            {
                case AUT.Uk:
                    var responce = Drive.Api.Queries.Post(GetFixedTermLoanTopupCalculationQuery.New(r =>{
                                                                                             r.ApplicationId = _application.Id;
                                                                                             r.TopupAmount = _amount;
                                                                                             }));

                    _interestAndFeesAmount = Convert.ToDouble(responce.Values["InterestAndFeesAmount"].Single());
                    _totalToRepay = Convert.ToDouble(responce.Values["TotalRepayable"].Single());
                    break;

                case AUT.Ca:
                case AUT.Pl:
                case AUT.Za:
                case AUT.Wb:
                    throw new System.ArgumentException("TopUp working only for Uk region, AUT is not correct", "Config AUT");
                default:
                    throw new NotImplementedException();
            }

            return new TopUp(_interestAndFeesAmount, _totalToRepay);
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
