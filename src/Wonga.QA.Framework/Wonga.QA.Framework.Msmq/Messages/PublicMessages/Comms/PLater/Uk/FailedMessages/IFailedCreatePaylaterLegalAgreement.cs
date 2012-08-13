using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.PLater.Uk.FailedMessages
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.FailedMessages.IFailedCreatePaylaterLegalAgreement </summary>
    [XmlRoot("IFailedCreatePaylaterLegalAgreement", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.FailedMessages", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms.PLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IFailedCreatePaylaterLegalAgreement : MsmqMessage<IFailedCreatePaylaterLegalAgreement>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
