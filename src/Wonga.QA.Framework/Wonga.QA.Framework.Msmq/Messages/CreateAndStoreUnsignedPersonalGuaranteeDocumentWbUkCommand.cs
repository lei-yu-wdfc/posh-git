using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PersonalGuarantee.CreateAndStoreUnsignedPersonalGuaranteeDocumentMessage </summary>
    [XmlRoot("CreateAndStoreUnsignedPersonalGuaranteeDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PersonalGuarantee", DataType = "")]
    public partial class CreateAndStoreUnsignedPersonalGuaranteeDocumentWbUkCommand : MsmqMessage<CreateAndStoreUnsignedPersonalGuaranteeDocumentWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
