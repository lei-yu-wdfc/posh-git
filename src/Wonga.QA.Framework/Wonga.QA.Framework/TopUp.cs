using System;

namespace Wonga.QA.Framework
{
    public class Topup
    {
		public Int32 Id;
        public int Amount { get; private set; }
        public Guid ApplicationId { get; private set; }
        public bool IsAccepted { get; private set; }
        public int DaysUntilRepaymentDate { get; private set; }

        public Topup(Int32 topUpId, Guid applicationId, bool isAccepted)
        {
        	Id = topUpId;
        	ApplicationId = applicationId;
        	IsAccepted = isAccepted;
        }
    }
}
