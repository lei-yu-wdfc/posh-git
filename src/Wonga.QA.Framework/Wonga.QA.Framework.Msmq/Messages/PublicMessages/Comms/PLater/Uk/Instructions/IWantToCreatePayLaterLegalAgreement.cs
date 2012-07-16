using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.PLater.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToCreatePayLaterLegalAgreement </summary>
    [XmlRoot("IWantToCreatePayLaterLegalAgreement", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterLegalAgreement : MsmqMessage<IWantToCreatePayLaterLegalAgreement>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
