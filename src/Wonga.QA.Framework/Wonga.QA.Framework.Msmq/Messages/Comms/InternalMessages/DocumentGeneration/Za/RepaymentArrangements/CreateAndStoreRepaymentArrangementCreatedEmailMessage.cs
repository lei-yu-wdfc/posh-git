using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateAndStoreRepaymentArrangementCreatedEmailMessage </summary>
    [XmlRoot("CreateAndStoreRepaymentArrangementCreatedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.DocumentGeneration.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateAndStoreRepaymentArrangementCreatedEmailMessage : MsmqMessage<CreateAndStoreRepaymentArrangementCreatedEmailMessage>
    {
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
