using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Email.Wb.Uk.IGuarantorFundsAdvancedEmailSent </summary>
    [XmlRoot("IGuarantorFundsAdvancedEmailSent", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk", DataType = "")]
    public partial class IGuarantorFundsAdvancedEmailSentWbUkEvent : MsmqMessage<IGuarantorFundsAdvancedEmailSentWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
