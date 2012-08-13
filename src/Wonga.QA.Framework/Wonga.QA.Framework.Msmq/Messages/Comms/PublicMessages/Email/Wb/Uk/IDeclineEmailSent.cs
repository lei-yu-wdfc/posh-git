using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Email.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Email.Wb.Uk.IDeclineEmailSent </summary>
    [XmlRoot("IDeclineEmailSent", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages.Email.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IDeclineEmailSent : MsmqMessage<IDeclineEmailSent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
