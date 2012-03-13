using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IUnsignedLoanAgreementAddedToLegalRepository </summary>
    [XmlRoot("IUnsignedLoanAgreementAddedToLegalRepository", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.ILoanAgreementAddedToLegalRepository")]
    public partial class IUnsignedLoanAgreementAddedToLegalRepositoryWbUkEvent : MsmqMessage<IUnsignedLoanAgreementAddedToLegalRepositoryWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
