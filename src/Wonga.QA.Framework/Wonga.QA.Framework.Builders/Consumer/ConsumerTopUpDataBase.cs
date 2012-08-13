using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Builders.Consumer
{
    public class ConsumerTopUpDataBase
    {
        public int amount;
        public Guid customerId;
        public Guid applicationId;
        public bool isEnabled;
        public double amountMax;
        public double totalToRepay;
        public double transmissionFee;
        public double interestRateMonthly;
        public double interestRateAnnual;
        public double interestAndFeesAmount;
        public int interestRateAPR;
        public int daysTillRepaymentDate;
        public string Currency;
        public DateTime updateOn;
        public Guid FixedTermLoanTopupId;
    }
}
