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
        public Boolean IsEnabled;
        public String LoanExtensionId;
        public Boolean HasStatusAccepted;
        public Date ExtendDate;
        public Decimal PartPaymentAmount;
        public Decimal OriginalBalance;
        public Decimal NewFinalBalance;

        public ConsumerLoanExtensionDataBase()
        {
            HasStatusAccepted = true;
            Term = DateTime.Now;
            IsEnabled = true;
            LoanExtensionId = (String)Guid.NewGuid().ToString();
            PartPaymentAmount = Get.RandomInt(10, 50);
        }
    }
}
