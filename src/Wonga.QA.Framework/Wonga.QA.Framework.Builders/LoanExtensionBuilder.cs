using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Builders.Consumer;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
    class LoanExtensionBuilder
    {
        public static class Consumer
        {
            public static ConsumerLoanExtensionBuilderBase New(Guid customerId, Guid applicationId, DateTime term, double partPaymentAmount)
            {
                switch (Config.AUT)
                {
                    case AUT.Uk:
                        return new Builders.Consumer.Uk.ConsumerLoanExtensionBuilder(customerId, applicationId, term, partPaymentAmount);
                    default:
                        throw new NotSupportedException(Config.AUT.ToString());
                }
            }
        }
    }
}
