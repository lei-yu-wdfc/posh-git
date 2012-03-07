using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.IRiskAccountMobilePhoneAdded </summary>
    [XmlRoot("IRiskAccountMobilePhoneAdded", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IRiskAccountMobilePhoneAddedEvent : MsmqMessage<IRiskAccountMobilePhoneAddedEvent>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
