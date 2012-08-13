using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreExtensionAgreedEmail </summary>
    [XmlRoot("IWantToCreateAndStoreExtensionAgreedEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStoreExtensionAgreedEmail : MsmqMessage<IWantToCreateAndStoreExtensionAgreedEmail>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AgreementFileId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
