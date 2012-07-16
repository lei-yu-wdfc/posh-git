using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Events.Wb.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Wb.Uk.IUnsignedLegalAgreementCreatedAndStoredTranslated </summary>
    [XmlRoot("IUnsignedLegalAgreementCreatedAndStoredTranslated", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IUnsignedLegalAgreementCreatedAndStoredTranslated : MsmqMessage<IUnsignedLegalAgreementCreatedAndStoredTranslated>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
