using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToCreatePayLaterSecciAgreementResponse </summary>
    [XmlRoot("IWantToCreatePayLaterSecciAgreementResponse", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterSecciAgreementResponseUkEvent : MsmqMessage<IWantToCreatePayLaterSecciAgreementResponseUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
