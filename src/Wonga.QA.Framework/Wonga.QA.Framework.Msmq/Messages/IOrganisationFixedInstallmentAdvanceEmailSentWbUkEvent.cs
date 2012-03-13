using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IOrganisationFixedInstallmentAdvanceEmailSent </summary>
    [XmlRoot("IOrganisationFixedInstallmentAdvanceEmailSent", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.IEmailSent")]
    public partial class IOrganisationFixedInstallmentAdvanceEmailSentWbUkEvent : MsmqMessage<IOrganisationFixedInstallmentAdvanceEmailSentWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
    }
}
