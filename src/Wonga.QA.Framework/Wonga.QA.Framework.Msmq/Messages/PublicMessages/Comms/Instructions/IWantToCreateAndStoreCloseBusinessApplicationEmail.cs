using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreCloseBusinessApplicationEmail </summary>
    [XmlRoot("IWantToCreateAndStoreCloseBusinessApplicationEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStoreCloseBusinessApplicationEmail : MsmqMessage<IWantToCreateAndStoreCloseBusinessApplicationEmail>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
