using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Ca.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Ca.Instructions.IWantToGenerateAConfirmationOfPreAuthorisedDebitDocument </summary>
    [XmlRoot("IWantToGenerateAConfirmationOfPreAuthorisedDebitDocument", Namespace = "Wonga.PublicMessages.Comms.Ca.Instructions", DataType = "Wonga.PublicMessages.Comms.Instructions.IWantToGenerateALegalDocument" )
    , SourceAssembly("Wonga.PublicMessages.Comms.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToGenerateAConfirmationOfPreAuthorisedDebitDocument : MsmqMessage<IWantToGenerateAConfirmationOfPreAuthorisedDebitDocument>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
