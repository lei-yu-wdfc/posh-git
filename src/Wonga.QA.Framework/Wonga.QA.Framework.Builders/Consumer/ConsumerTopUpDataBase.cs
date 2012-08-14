using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;

namespace Wonga.QA.Framework.Builders.Consumer
{
    public class ConsumerTopUpDataBase
    {
        public Int32 amount;
        public String customerId;
        public String applicationId;
        public Boolean isEnabled;
        public Decimal amountMax;
        public Decimal totalToRepay;
        public Decimal transmissionFee;
        public Decimal interestRateMonthly;
        public Decimal interestRateAnnual;
        public Decimal interestAndFeesAmount;
        public Int32 interestRateAPR;
        public Int32 daysTillRepaymentDate;
        public String Currency;
        public Date updateOn;
        public String FixedTermLoanTopupId;
    }
}
