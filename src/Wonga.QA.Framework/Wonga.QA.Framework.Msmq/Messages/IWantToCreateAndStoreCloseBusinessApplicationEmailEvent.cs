using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreCloseBusinessApplicationEmail </summary>
    [XmlRoot("IWantToCreateAndStoreCloseBusinessApplicationEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreCloseBusinessApplicationEmailEvent : MsmqMessage<IWantToCreateAndStoreCloseBusinessApplicationEmailEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
