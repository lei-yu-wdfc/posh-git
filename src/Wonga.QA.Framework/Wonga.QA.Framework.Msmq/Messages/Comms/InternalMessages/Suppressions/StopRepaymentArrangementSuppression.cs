using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Suppressions
{
    /// <summary> Wonga.Comms.InternalMessages.Suppressions.StopRepaymentArrangementSuppression </summary>
    [XmlRoot("StopRepaymentArrangementSuppression", Namespace = "Wonga.Comms.InternalMessages.Suppressions", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class StopRepaymentArrangementSuppression : MsmqMessage<StopRepaymentArrangementSuppression>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
