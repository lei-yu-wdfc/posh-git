using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.PayLater.PublicMessages
{
    /// <summary> Wonga.Risk.PayLater.PublicMessages.IRiskPayLaterApplicationAccepted </summary>
    [XmlRoot("IRiskPayLaterApplicationAccepted", Namespace = "Wonga.Risk.PayLater.PublicMessages", DataType = "Wonga.Risk.IAcceptedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.PayLater.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskPayLaterApplicationAccepted : MsmqMessage<IRiskPayLaterApplicationAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
