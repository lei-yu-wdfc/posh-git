using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreatePreApproveDirectDebitMessage", Namespace = "Wonga.Comms.InternalMessages.Ca", DataType = "")]
    public partial class CreatePreApproveDirectDebitCaCommand : MsmqMessage<CreatePreApproveDirectDebitCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
