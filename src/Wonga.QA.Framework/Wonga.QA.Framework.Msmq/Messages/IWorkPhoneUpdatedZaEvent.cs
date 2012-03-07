using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IWorkPhoneUpdated </summary>
    [XmlRoot("IWorkPhoneUpdated", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public partial class IWorkPhoneUpdatedZaEvent : MsmqMessage<IWorkPhoneUpdatedZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
