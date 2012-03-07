using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk.IUnsignedLegalAgreementCreatedAndStored </summary>
    [XmlRoot("IUnsignedLegalAgreementCreatedAndStored", Namespace = "Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IUnsignedLegalAgreementCreatedAndStoredWbUkEvent : MsmqMessage<IUnsignedLegalAgreementCreatedAndStoredWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
    }
}
