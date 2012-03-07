using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IGuarantorDataCorrelated </summary>
    [XmlRoot("IGuarantorDataCorrelated", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IGuarantorDataCorrelatedWbUkEvent : MsmqMessage<IGuarantorDataCorrelatedWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PrimaryDirectorLoanAgreementDocumentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
