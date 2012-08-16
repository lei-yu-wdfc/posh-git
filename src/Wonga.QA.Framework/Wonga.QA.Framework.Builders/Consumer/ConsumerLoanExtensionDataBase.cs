using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
    class ConsumerLoanExtensionDataBase
    {
        public DateTime Term;
        public Decimal PartPaymentAmount;

        public ConsumerLoanExtensionDataBase()
        {
            Term = DateTime.Now;
            PartPaymentAmount = Get.RandomInt(10, 50);
        }
    }
}
