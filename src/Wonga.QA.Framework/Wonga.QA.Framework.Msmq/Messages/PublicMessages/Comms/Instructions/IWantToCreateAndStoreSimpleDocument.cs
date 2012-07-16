using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreSimpleDocument </summary>
    [XmlRoot("IWantToCreateAndStoreSimpleDocument", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreSimpleDocument : MsmqMessage<IWantToCreateAndStoreSimpleDocument>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OriginatingSagaId { get; set; }
        public SimpleEmailEnum SimpleEmailType { get; set; }
    }
}
