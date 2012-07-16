using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskAccountMobilePhoneAdded </summary>
    [XmlRoot("IRiskAccountMobilePhoneAdded", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IRiskAccountMobilePhoneAdded : MsmqMessage<IRiskAccountMobilePhoneAdded>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
