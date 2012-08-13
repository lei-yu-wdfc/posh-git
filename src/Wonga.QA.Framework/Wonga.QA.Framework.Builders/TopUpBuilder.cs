using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Builders.Consumer;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders
{
    public class TopUpBuilder
    {
        public static class Consumer
        {
            public static ConsumerTopUpBaseBuilder New(Guid customerId, Guid applicationId, int amount)
            {
                switch (Config.AUT)
                {
                    case AUT.Uk:
                        return new Builders.Consumer.Uk.ConsumerTopUpBuilder(customerId, applicationId, amount);
                    default:
                        throw new NotSupportedException(Config.AUT.ToString());
                }
            }
        }

    }
}
