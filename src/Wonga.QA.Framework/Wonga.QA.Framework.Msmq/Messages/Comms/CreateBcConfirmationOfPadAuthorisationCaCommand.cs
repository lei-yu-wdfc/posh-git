using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateBcConfirmationOfPADAuthorisationMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.BritishColombia", DataType = "")]
    public class CreateBcConfirmationOfPadAuthorisationCaCommand : MsmqMessage<CreateBcConfirmationOfPadAuthorisationCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
