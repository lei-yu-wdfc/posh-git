using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
    public class ConsumerTopUpBaseBuilder
    {
        protected Customer _customer;
        protected Application _application;
        protected int _amount;
        protected double _interestAndFeesAmount;
        protected double _totalToRepay;

        public ConsumerTopUpBaseBuilder(Customer customer, Application application, int amount)
        {
            _customer = customer;
            _application = application;
            _amount = amount;
        }
    }
}
