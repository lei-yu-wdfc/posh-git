using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.BritishColombia.CreateBcConfirmationOfPADAuthorisationMessage </summary>
    [XmlRoot("CreateBcConfirmationOfPADAuthorisationMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.BritishColombia", DataType = "")]
    public partial class CreateBcConfirmationOfPadAuthorisationCaCommand : MsmqMessage<CreateBcConfirmationOfPadAuthorisationCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
