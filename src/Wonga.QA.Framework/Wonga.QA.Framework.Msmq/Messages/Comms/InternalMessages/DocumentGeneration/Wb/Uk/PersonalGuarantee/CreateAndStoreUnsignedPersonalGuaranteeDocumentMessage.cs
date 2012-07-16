using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PersonalGuarantee
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PersonalGuarantee.CreateAndStoreUnsignedPersonalGuaranteeDocumentMessage </summary>
    [XmlRoot("CreateAndStoreUnsignedPersonalGuaranteeDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PersonalGuarantee", DataType = "")]
    public partial class CreateAndStoreUnsignedPersonalGuaranteeDocumentMessage : MsmqMessage<CreateAndStoreUnsignedPersonalGuaranteeDocumentMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
