using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IGuarantorDataCorrelatedInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.IGuarantorDataCorrelated,Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IGuarantorDataCorrelatedInternalWbUkEvent : MsmqMessage<IGuarantorDataCorrelatedInternalWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PrimaryDirectorLoanAgreementDocumentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
