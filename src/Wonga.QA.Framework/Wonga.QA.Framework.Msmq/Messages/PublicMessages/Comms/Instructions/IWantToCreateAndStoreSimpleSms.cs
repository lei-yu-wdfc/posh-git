using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreSimpleSms </summary>
    [XmlRoot("IWantToCreateAndStoreSimpleSms", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreSimpleSms : MsmqMessage<IWantToCreateAndStoreSimpleSms>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OriginatingSagaId { get; set; }
        public SimpleSmsEnum SimpleSmsType { get; set; }
    }
}
