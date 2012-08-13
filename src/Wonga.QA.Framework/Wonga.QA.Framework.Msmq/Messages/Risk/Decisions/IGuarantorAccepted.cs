using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Decisions
{
    /// <summary> Wonga.Risk.Decisions.IGuarantorAccepted </summary>
    [XmlRoot("IGuarantorAccepted", Namespace = "Wonga.Risk.Decisions", DataType = "Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IGuarantorAccepted : MsmqMessage<IGuarantorAccepted>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
