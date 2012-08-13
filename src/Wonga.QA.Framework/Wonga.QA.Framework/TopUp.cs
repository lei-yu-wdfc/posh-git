using System;
using Wonga.QA.Framework.Old;

namespace Wonga.QA.Framework
{
    public class TopUp
    {
        public int amount { get; set; }
        public Customer customer { get; set; }
        public Application application { get; set; }
        public bool isEnabled { get; set; }
        public double amountMax { get; set; }
        public double totalToRepay { get; set; }
        public double transmissionFee { get; set; }
        public double interestRateMonthly { get; set; }
        public double interestRateAnnual { get; set; }
        public double interestAndFeesAmount { get; set; }
        public int interestRateAPR { get; set; }
        public int daysTillRepaymentDate { get; set; }
        public string Currency { get; set; }
        public DateTime updateOn { get; set; }
        public Guid FixedTermLoanTopupId;

        public TopUp(double _interestAndFeesAmount, double _totalToRepay, Guid _TopUpId)
        {
            interestAndFeesAmount = _interestAndFeesAmount;
            totalToRepay = _totalToRepay;
            FixedTermLoanTopupId = _TopUpId;
        }
    }
}
