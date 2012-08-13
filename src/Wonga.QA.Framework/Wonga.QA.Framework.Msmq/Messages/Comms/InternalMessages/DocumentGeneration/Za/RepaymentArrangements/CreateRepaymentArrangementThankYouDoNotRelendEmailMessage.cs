using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateRepaymentArrangementThankYouDoNotRelendEmailMessage </summary>
    [XmlRoot("CreateRepaymentArrangementThankYouDoNotRelendEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.DocumentGeneration.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateRepaymentArrangementThankYouDoNotRelendEmailMessage : MsmqMessage<CreateRepaymentArrangementThankYouDoNotRelendEmailMessage>
    {
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
