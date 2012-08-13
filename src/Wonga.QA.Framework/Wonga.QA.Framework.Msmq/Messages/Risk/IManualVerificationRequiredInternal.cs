using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IManualVerificationRequiredInternal </summary>
    [XmlRoot("IManualVerificationRequiredInternal", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IManualVerificationRequired,Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IManualVerificationRequiredInternal : MsmqMessage<IManualVerificationRequiredInternal>
    {
        public Guid WorkflowSagaId { get; set; }
        public String RequestType { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
