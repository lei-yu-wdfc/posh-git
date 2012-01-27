using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ExtendLoanStartedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class ExtendLoanStartedInternalCommand : MsmqMessage<ExtendLoanStartedInternalCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public DateTime ExtendDate { get; set; }
        public Decimal PartPaymentRequired { get; set; }
        public Decimal NewFinalBalance { get; set; }
    }
}
