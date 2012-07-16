using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreCloseBusinessApplicationEmail </summary>
    [XmlRoot("IWantToCreateAndStoreCloseBusinessApplicationEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreCloseBusinessApplicationEmail : MsmqMessage<IWantToCreateAndStoreCloseBusinessApplicationEmail>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
