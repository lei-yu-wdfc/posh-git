using System;

namespace Wonga.QA.Framework.Builders.Consumer
{
    public class ConsumerTopupDataBase
    {
        public Int32 Amount;
        public Boolean StatusAccepted;

        public ConsumerTopupDataBase()
        {
            StatusAccepted = true;
            Amount = 100;
        }
    }
}
