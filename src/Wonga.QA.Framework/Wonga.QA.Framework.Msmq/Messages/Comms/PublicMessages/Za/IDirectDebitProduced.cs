using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Za
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IDirectDebitProduced </summary>
    [XmlRoot("IDirectDebitProduced", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public partial class IDirectDebitProduced : MsmqMessage<IDirectDebitProduced>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
