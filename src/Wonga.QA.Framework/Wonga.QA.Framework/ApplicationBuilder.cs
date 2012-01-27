using System;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework
{
    public class ApplicationBuilder
    {
        private Guid _id;
        private Customer _customer;

        private ApplicationBuilder()
        {
            _id = Guid.NewGuid();
        }

        public static ApplicationBuilder New(Customer customer)
        {
            return new ApplicationBuilder { _customer = customer };
        }

        public Application Build()
        {
            Drivers.Api.Commands.Post(new ApiRequest[]
            {
                SubmitApplicationBehaviourCommand.Random(r => r.ApplicationId = _id),
                CreateFixedTermLoanApplicationCommand.Random(r => { r.ApplicationId = _id; r.AccountId = _customer.Id;}),
                VerifyFixedTermLoanCommand.Random(r=> { r.ApplicationId = _id; r.AccountId=_customer.Id; })
            });

            return new Application(_id);
        }
    }
}
