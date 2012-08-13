using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Events
{
    /// <summary> Wonga.Risk.InternalMessages.Events.IDecisionDetails </summary>
    [XmlRoot("IDecisionDetails", Namespace = "Wonga.Risk.InternalMessages.Events", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Events, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IDecisionDetails : MsmqMessage<IDecisionDetails>
    {
        public Guid ApplicationId { get; set; }
        public String FailedCheckpoint { get; set; }
        public Int32? RepaymentPredictionScore { get; set; }
    }
}
