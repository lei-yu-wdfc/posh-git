using System;
using Wonga.QA.Framework.Builders.Consumer;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders
{
    public class TopupBuilder
    {
        public static class Consumer
        {
            public static ConsumerTopupBuilderBase New(Guid applicationId, int amount)
            {
                switch (Config.AUT)
                {
                    case AUT.Uk:
                        return new Builders.Consumer.Uk.ConsumerTopupBuilder(applicationId, new ConsumerTopupDataBase());
                    default:
                        throw new NotSupportedException(Config.AUT.ToString());
                }
            }
        }

    }
}
