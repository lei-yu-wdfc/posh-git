using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages.Wb.Uk
{
    /// <summary> Wonga.Email.InternalMessages.Wb.Uk.SendOrganisationFixedInstallmentAdvanceEmailMessage </summary>
    [XmlRoot("SendOrganisationFixedInstallmentAdvanceEmailMessage", Namespace = "Wonga.Email.InternalMessages.Wb.Uk", DataType = "")]
    public partial class SendOrganisationFixedInstallmentAdvanceEmailMessage : MsmqMessage<SendOrganisationFixedInstallmentAdvanceEmailMessage>
    {
        public Guid ApplicationId { get; set; }
        public Guid EmailContentDocumentId { get; set; }
        public String CustomerEmail { get; set; }
    }
}
