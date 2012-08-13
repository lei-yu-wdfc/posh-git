using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskApplicationGuarantorAdded </summary>
    [XmlRoot("IRiskApplicationGuarantorAdded", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskApplicationGuarantorAdded : MsmqMessage<IRiskApplicationGuarantorAdded>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
