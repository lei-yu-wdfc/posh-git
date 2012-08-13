using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Risk.PayLater.Uk
{
    /// <summary> Wonga.PublicMessages.Risk.PayLater.Uk.IApplicationAccepted </summary>
    [XmlRoot("IApplicationAccepted", Namespace = "Wonga.PublicMessages.Risk.PayLater.Uk", DataType = "Wonga.Risk.IAcceptedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.PublicMessages.Risk.PayLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IApplicationAccepted : MsmqMessage<IApplicationAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
