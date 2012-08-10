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
            public static ConsumerTopUpBaseBuilder New(Customer customer = null, Application application = null, int amount = 100)
            {
                customer = customer ?? CustomerBuilder.New().Build();
                application = application ?? ApplicationBuilder.Consumer.New(customer).Build();

                switch (Config.AUT)
                {
                    case AUT.Uk:
                        return new Builders.Consumer.Uk.CunsomerTopUpBuilder(customer, application, amount);
                    default:
                        throw new NotSupportedException(Config.AUT.ToString());
                }
            }
        }

    }
}
