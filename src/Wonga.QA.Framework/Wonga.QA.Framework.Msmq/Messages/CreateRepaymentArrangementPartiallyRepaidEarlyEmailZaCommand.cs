using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateRepaymentArrangementPartiallyRepaidEarlyEmailMessage </summary>
    [XmlRoot("CreateRepaymentArrangementPartiallyRepaidEarlyEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateRepaymentArrangementPartiallyRepaidEarlyEmailZaCommand : MsmqMessage<CreateRepaymentArrangementPartiallyRepaidEarlyEmailZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
