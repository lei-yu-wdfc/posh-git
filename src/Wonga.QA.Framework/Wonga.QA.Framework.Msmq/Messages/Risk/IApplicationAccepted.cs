using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IApplicationAccepted </summary>
    [XmlRoot("IApplicationAccepted", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IAcceptedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent")]
    public partial class IApplicationAccepted : MsmqMessage<IApplicationAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
