using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.UpdatedApplicationClosedOn </summary>
    [XmlRoot("UpdatedApplicationClosedOn", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdatedApplicationClosedOn : MsmqMessage<UpdatedApplicationClosedOn>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime ClosedOn { get; set; }
    }
}
