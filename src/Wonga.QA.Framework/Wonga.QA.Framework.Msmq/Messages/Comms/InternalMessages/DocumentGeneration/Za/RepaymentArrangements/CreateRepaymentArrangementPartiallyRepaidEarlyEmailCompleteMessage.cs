using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateRepaymentArrangementPartiallyRepaidEarlyEmailCompleteMessage </summary>
    [XmlRoot("CreateRepaymentArrangementPartiallyRepaidEarlyEmailCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.DocumentGeneration.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateRepaymentArrangementPartiallyRepaidEarlyEmailCompleteMessage : MsmqMessage<CreateRepaymentArrangementPartiallyRepaidEarlyEmailCompleteMessage>
    {
        public Guid AccountId { get; set; }
        public Byte[] HtmlContent { get; set; }
        public Byte[] PlainContent { get; set; }
    }
}
