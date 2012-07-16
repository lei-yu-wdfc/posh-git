using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Ca.Instructions.Alberta
{
    /// <summary> Wonga.PublicMessages.Comms.Ca.Instructions.Alberta.IWantToGenerateAPreAuthorisedDebitDocument </summary>
    [XmlRoot("IWantToGenerateAPreAuthorisedDebitDocument", Namespace = "Wonga.PublicMessages.Comms.Ca.Instructions.Alberta", DataType = "Wonga.PublicMessages.Comms.Instructions.IWantToGenerateALegalDocument")]
    public partial class IWantToGenerateAPreAuthorisedDebitDocument : MsmqMessage<IWantToGenerateAPreAuthorisedDebitDocument>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
