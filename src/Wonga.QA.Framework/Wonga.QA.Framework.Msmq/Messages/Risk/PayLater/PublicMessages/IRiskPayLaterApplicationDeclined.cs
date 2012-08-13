using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.PayLater.PublicMessages
{
    /// <summary> Wonga.Risk.PayLater.PublicMessages.IRiskPayLaterApplicationDeclined </summary>
    [XmlRoot("IRiskPayLaterApplicationDeclined", Namespace = "Wonga.Risk.PayLater.PublicMessages", DataType = "Wonga.Risk.IDeclinedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.PayLater.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskPayLaterApplicationDeclined : MsmqMessage<IRiskPayLaterApplicationDeclined>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
