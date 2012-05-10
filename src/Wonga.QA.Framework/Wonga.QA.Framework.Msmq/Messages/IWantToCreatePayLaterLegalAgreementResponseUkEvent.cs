using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToCreatePayLaterLegalAgreementResponse </summary>
    [XmlRoot("IWantToCreatePayLaterLegalAgreementResponse", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterLegalAgreementResponseUkEvent : MsmqMessage<IWantToCreatePayLaterLegalAgreementResponseUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
