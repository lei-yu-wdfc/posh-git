using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.PLater.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToCreatePayLaterSecciAgreement </summary>
    [XmlRoot("IWantToCreatePayLaterSecciAgreement", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterSecciAgreement : MsmqMessage<IWantToCreatePayLaterSecciAgreement>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
