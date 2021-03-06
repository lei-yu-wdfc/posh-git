using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IRepaymentArrangementPaymentMissedSMS </summary>
    [XmlRoot("IRepaymentArrangementPaymentMissedSMS", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRepaymentArrangementPaymentMissedSMS : MsmqMessage<IRepaymentArrangementPaymentMissedSMS>
    {
        public Guid AccountId { get; set; }
        public Guid OriginatingSagaId { get; set; }
        public String ToNumberFormatted { get; set; }
        public String MessageText { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
