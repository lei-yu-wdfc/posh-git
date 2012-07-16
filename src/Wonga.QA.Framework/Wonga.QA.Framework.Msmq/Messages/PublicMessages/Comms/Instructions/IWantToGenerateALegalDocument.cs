using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToGenerateALegalDocument </summary>
    [XmlRoot("IWantToGenerateALegalDocument", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToGenerateALegalDocument : MsmqMessage<IWantToGenerateALegalDocument>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
