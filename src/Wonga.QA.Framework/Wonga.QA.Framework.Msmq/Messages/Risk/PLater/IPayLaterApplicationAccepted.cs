using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.PLater
{
    /// <summary> Wonga.Risk.PLater.IPayLaterApplicationAccepted </summary>
    [XmlRoot("IPayLaterApplicationAccepted", Namespace = "Wonga.Risk.PLater", DataType = "Wonga.Risk.IApplicationAccepted,Wonga.Risk.IAcceptedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent")]
    public partial class IPayLaterApplicationAccepted : MsmqMessage<IPayLaterApplicationAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
