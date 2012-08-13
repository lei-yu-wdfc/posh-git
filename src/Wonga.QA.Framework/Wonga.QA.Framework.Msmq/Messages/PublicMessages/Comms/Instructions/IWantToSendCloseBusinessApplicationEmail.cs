using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendCloseBusinessApplicationEmail </summary>
    [XmlRoot("IWantToSendCloseBusinessApplicationEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendCloseBusinessApplicationEmail : MsmqMessage<IWantToSendCloseBusinessApplicationEmail>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Object AccountIdFileIdMappings { get; set; }
    }
}
