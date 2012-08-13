using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.PLater.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToSendPayLaterApplicationConfirmationEmailResponse </summary>
    [XmlRoot("IWantToSendPayLaterApplicationConfirmationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms.PLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendPayLaterApplicationConfirmationEmailResponse : MsmqMessage<IWantToSendPayLaterApplicationConfirmationEmailResponse>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
