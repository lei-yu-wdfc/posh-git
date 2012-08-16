using System;

namespace Wonga.QA.Framework
{
    public class Topup
    {
		public Guid Id;
        public int Amount { get; private set; }
        public Guid ApplicationId { get; private set; }
        public bool IsAccepted { get; private set; }
        public int DaysUntilRepaymentDate { get; private set; }

        public Topup(Guid topUpId, Guid applicationId, bool isAccepted)
        {
        	Id = topUpId;
        	ApplicationId = applicationId;
        	IsAccepted = isAccepted;
        }
    }
}
