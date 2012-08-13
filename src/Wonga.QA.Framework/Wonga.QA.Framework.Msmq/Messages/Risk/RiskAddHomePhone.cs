using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.RiskAddHomePhone </summary>
    [XmlRoot("RiskAddHomePhone", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RiskAddHomePhone : MsmqMessage<RiskAddHomePhone>
    {
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
