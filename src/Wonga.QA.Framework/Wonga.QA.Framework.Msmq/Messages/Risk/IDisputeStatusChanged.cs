using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IDisputeStatusChanged </summary>
    [XmlRoot("IDisputeStatusChanged", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IDisputeStatusChanged : MsmqMessage<IDisputeStatusChanged>
    {
        public Guid AccountId { get; set; }
        public Boolean HasDispute { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
