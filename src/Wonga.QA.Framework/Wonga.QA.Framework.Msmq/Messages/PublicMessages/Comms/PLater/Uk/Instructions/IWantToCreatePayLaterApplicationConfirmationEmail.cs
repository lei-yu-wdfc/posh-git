using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.PLater.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToCreatePayLaterApplicationConfirmationEmail </summary>
    [XmlRoot("IWantToCreatePayLaterApplicationConfirmationEmail", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms.PLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreatePayLaterApplicationConfirmationEmail : MsmqMessage<IWantToCreatePayLaterApplicationConfirmationEmail>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
