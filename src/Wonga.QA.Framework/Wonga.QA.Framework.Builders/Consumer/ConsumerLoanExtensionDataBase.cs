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
        public String CustomerId;
        public String ApplicationId;
        public Boolean IsEnabled;
        public String LoanExtensionId;
        public Boolean HasStatusAccepted;
        public Date ExtendDate;
        public String PaymentCardId;
        public String PaymentCardCv2;
        public Decimal PartPaymentAmount;
        public Decimal TodaysBalance;
        public Decimal OriginalBalance;
        public Decimal NewFinalBalance;

        public ConsumerLoanExtensionDataBase()
        {
            HasStatusAccepted = true;
            Term = DateTime.Now;
            IsEnabled = true;
            LoanExtensionId = (String)Guid.NewGuid().ToString();
            PartPaymentAmount = Get.RandomInt(10, 50);
            PaymentCardId = Guid.NewGuid().ToString();
        }
    }
}
