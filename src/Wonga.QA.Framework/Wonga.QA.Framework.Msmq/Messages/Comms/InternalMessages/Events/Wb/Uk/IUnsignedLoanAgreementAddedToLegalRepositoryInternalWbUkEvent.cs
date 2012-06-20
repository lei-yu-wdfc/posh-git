using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Events.Wb.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Wb.Uk.IUnsignedLoanAgreementAddedToLegalRepositoryInternal </summary>
    [XmlRoot("IUnsignedLoanAgreementAddedToLegalRepositoryInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.IUnsignedLoanAgreementAddedToLegalRepository,Wonga.Comms.PublicMessages.Wb.Uk.ILoanAgreementAddedToLegalRepository")]
    public partial class IUnsignedLoanAgreementAddedToLegalRepositoryInternalWbUkEvent : MsmqMessage<IUnsignedLoanAgreementAddedToLegalRepositoryInternalWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
