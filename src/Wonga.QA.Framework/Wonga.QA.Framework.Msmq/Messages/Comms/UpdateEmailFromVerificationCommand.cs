using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("UpdateEmailFromVerificationMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class UpdateEmailFromVerificationCommand : MsmqMessage<UpdateEmailFromVerificationCommand>
    {
        public Guid ChangeId { get; set; }
    }
}
