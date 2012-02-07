using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IConfirmationOfPADAuthorisationProduced", Namespace = "Wonga.Comms.PublicMessages.Ca", DataType = "")]
    public partial class IConfirmationOfPadAuthorisationProducedCaEvent : MsmqMessage<IConfirmationOfPadAuthorisationProducedCaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
