using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskGuarantorPartialDetailAdded </summary>
    [XmlRoot("IRiskGuarantorPartialDetailAdded", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IRiskGuarantorPartialDetailAddedEvent : MsmqMessage<IRiskGuarantorPartialDetailAddedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
