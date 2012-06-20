using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreDeclineEmail </summary>
    [XmlRoot("IWantToCreateAndStoreDeclineEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreDeclineEmailEvent : MsmqMessage<IWantToCreateAndStoreDeclineEmailEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String DeclineAdviceUrl { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
