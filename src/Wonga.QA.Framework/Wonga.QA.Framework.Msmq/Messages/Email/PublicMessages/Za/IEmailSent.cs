using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.PublicMessages.Za
{
    /// <summary> Wonga.Email.PublicMessages.Za.IEmailSent </summary>
    [XmlRoot("IEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "" )
    , SourceAssembly("Wonga.Email.PublicMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IEmailSent : MsmqMessage<IEmailSent>
    {
        public Guid AccountId { get; set; }
    }
}
